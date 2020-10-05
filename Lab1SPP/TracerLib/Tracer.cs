using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace TracerLib
{
    public class MethodTraceResultClass
    {
        public string methodName;
        public string className;
        public long execTime;
        public int threadId;
        public Stopwatch stopwatch;
        public List<MethodTraceResultClass> calledMethods;
    }

    public class ThreadTraceResultClass
    {
        public int id;
        public long execTime;
        public List<MethodTraceResultClass> calledMethods;
        //[NonSerialized]
        //public List<MethodTraceResult> stack;
    }
    [Serializable]
    public struct MethodTraceResult
    {
        public string methodName;
        public string className;
        public long execTime;
        public int threadId;
        public List<MethodTraceResult> calledMethods;
    }
    [Serializable]
    public struct ThreadTraceResult
    {
        public int id;
        public long execTime;
        public List<MethodTraceResult> calledMethods;
    }

    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        List<ThreadTraceResult> GetTraceResult();

    }
    public class Tracer : ITracer
    {
        private List<MethodTraceResultClass> _methodsTraceList;
        //public ReadOnlyCollection<MethodTraceResultClass> methodsTraceList;

        private List<ThreadTraceResultClass> _traceList;
        //public ReadOnlyCollection<ThreadTraceResultClass> traceList;

        //private List<TraceInfo> _traceInfo;

        public object locker;

        public Tracer()
        {
            _methodsTraceList = new List<MethodTraceResultClass>();
            //methodsTraceList = _methodsTraceList.AsReadOnly();
            _traceList = new List<ThreadTraceResultClass>();
            //traceList = _traceList.AsReadOnly();
            locker = new object();
        }

        private long CountThreadExecTime(List<MethodTraceResultClass> methodList)
        {
            long res = 0;
            foreach (MethodTraceResultClass mtr in methodList)
            {
                res += mtr.execTime;
            }
            
            return res;
        }
        

        private void ToFormTraceList(List<MethodTraceResultClass> methodTraceList, List<ThreadTraceResultClass> threadsList)
        {
            ThreadTraceResultClass bufThreadTraceResult;

            foreach (MethodTraceResultClass mtr in methodTraceList)
            {
                bufThreadTraceResult = _traceList.Find(x => x.id == mtr.threadId);
                if (bufThreadTraceResult != null)
                {
                    int ind = bufThreadTraceResult.calledMethods.IndexOf(mtr);
                    if (ind < 0)
                        bufThreadTraceResult.calledMethods.Add(mtr);
                }
                else
                {
                    bufThreadTraceResult = new ThreadTraceResultClass
                    {
                        id = mtr.threadId,
                        calledMethods = new List<MethodTraceResultClass>()
                    };
                    bufThreadTraceResult.calledMethods.Add(mtr);
                    threadsList.Add(bufThreadTraceResult);
                }
            }
            
        }

        private MethodTraceResultClass FindInList(List<MethodTraceResultClass> list, string methodName, string className, int threadId = -1)
        {
            if (className == null && threadId == -1)
            {
                return list.Find(x => x.methodName == methodName &&
                     x.threadId == Thread.CurrentThread.ManagedThreadId);
            }
            else if (className == null)
            {
                return list.Find(x => x.methodName == methodName && x.threadId == threadId);
            }
            else if (threadId == -1)
            {
                return list.Find(x => x.methodName == methodName && x.className == className &&
                     x.threadId == Thread.CurrentThread.ManagedThreadId);
            }
            else
            {
                return list.Find(x => x.methodName == methodName && x.className == className && x.threadId == threadId);
            }
            
        }

        private MethodTraceResultClass FindElement(List<MethodTraceResultClass> rootList, string methodName, string className, int threadId = -1)
        {
            MethodTraceResultClass result;

            result = FindInList(rootList, methodName, className, threadId);
            if (result != null)
                return result;

            foreach (MethodTraceResultClass tr in rootList)
            {
                if (tr.calledMethods.Count != 0)
                {
                    result = FindElement(tr.calledMethods, methodName, className, threadId);
                    if (result != null)
                        break;
                }
            }
            
            return result;
        }

        private struct MemberIdentifier
        {
            public string methodName;
            public string className;
        }

        private List<MemberIdentifier> MakeUpMethodCallTree()
        {
            List<MemberIdentifier> callTree = new List<MemberIdentifier>();
            StackTrace st = new StackTrace();
            for(int i = 4; i < st.FrameCount; i++)  //Стартовое i зависит от количества вызовов для обработки
            {
                StackFrame sf = st.GetFrame(i);
                MemberIdentifier callerMember = new MemberIdentifier();
                callerMember.methodName = sf.GetMethod().Name;
                callerMember.className = sf.GetMethod().DeclaringType.Name;
                if (callerMember.className == "ThreadHelper")  //Используется имя класса
                    break;

                callTree.Add(callerMember);
            }

            return callTree;
        }

        private void InsertToTraceTree(List<MethodTraceResultClass> rootList, MethodTraceResultClass elem)
        {
            List<MemberIdentifier> callTree = MakeUpMethodCallTree();
            MethodTraceResultClass traceResultElem;
            if (callTree.Count == 0)
                rootList.Add(elem);
            else
            {
                bool found = false;
                foreach (MemberIdentifier mi in callTree)
                {
                    traceResultElem = FindElement(rootList, mi.methodName, mi.className);
                    if (traceResultElem != null)
                    {
                        traceResultElem.calledMethods.Add(elem);
                        found = true;
                        break;
                    }
                    
                }
                if (!found)
                    rootList.Add(elem);
            }
        }
        public void StartTrace()
        {
            MethodTraceResultClass curTraceResult = new MethodTraceResultClass();

            StackFrame frame = new StackFrame(1);

            curTraceResult.methodName = frame.GetMethod().Name;
            curTraceResult.className = frame.GetMethod().DeclaringType.Name;            
            curTraceResult.threadId = Thread.CurrentThread.ManagedThreadId;
            curTraceResult.stopwatch = new Stopwatch();
            curTraceResult.execTime = 0;
            curTraceResult.calledMethods = new List<MethodTraceResultClass>();

            lock(locker)
            {
                InsertToTraceTree(_methodsTraceList, curTraceResult);
            }
            curTraceResult.stopwatch.Start();
        }
        public void StopTrace()
        {
            lock (locker)
            {
                StackFrame frame = new StackFrame(1);
                MethodTraceResultClass traceResultElem = FindElement(_methodsTraceList,
                    frame.GetMethod().Name, frame.GetMethod().DeclaringType.Name, Thread.CurrentThread.ManagedThreadId);
                if (traceResultElem != null)
                {
                    traceResultElem.stopwatch.Stop();
                    traceResultElem.execTime = traceResultElem.stopwatch.ElapsedMilliseconds;
                    ToFormTraceList(_methodsTraceList, _traceList);
                    ThreadTraceResultClass curThread = _traceList.Find(x => x.id == Thread.CurrentThread.ManagedThreadId);
                    curThread.execTime = CountThreadExecTime(curThread.calledMethods);
                } 
            }
        }

        private void CopyMethodsList(List<MethodTraceResult> destList, List<MethodTraceResultClass> sourceList)
        {
            foreach (MethodTraceResultClass mtrc in sourceList)
            {
                MethodTraceResult newMtr = new MethodTraceResult();
                newMtr.methodName = mtrc.methodName;
                newMtr.className = mtrc.className;
                newMtr.threadId = mtrc.threadId;
                newMtr.execTime = mtrc.execTime;
                newMtr.calledMethods = new List<MethodTraceResult>();
                if (mtrc.calledMethods.Count > 0)
                    CopyMethodsList(newMtr.calledMethods, mtrc.calledMethods);

                destList.Add(newMtr);
            }
        }

        private List<ThreadTraceResult> ToFormResultList()
        {
            List<ThreadTraceResult> resList = new List<ThreadTraceResult>();
            lock (locker)
            {
                foreach (ThreadTraceResultClass ttrc in _traceList)
                {
                    ThreadTraceResult newTtr = new ThreadTraceResult();
                    newTtr.id = ttrc.id;
                    newTtr.execTime = ttrc.execTime;
                    newTtr.calledMethods = new List<MethodTraceResult>();
                    if (ttrc.calledMethods.Count > 0)
                        CopyMethodsList(newTtr.calledMethods, ttrc.calledMethods);

                    resList.Add(newTtr);
                }
            }
            return resList;
        }

        public List<ThreadTraceResult> GetTraceResult()
        {
            return ToFormResultList();
        }
        
    }
    public abstract class Serializer
    {
        public abstract byte[] SerializeObj(List<ThreadTraceResult> traceList);
    }    

    public class JsonSerializer : Serializer
    {        
        public override byte[] SerializeObj(List<ThreadTraceResult> traceList)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };

            string Serialized = JsonConvert.SerializeObject(traceList, settings);
            byte[] ret = System.Text.Encoding.UTF8.GetBytes(Serialized);
            return ret;
            
        }
    }

    public class XmlSerializer: Serializer
    {
        public override byte[] SerializeObj(List<ThreadTraceResult> traceList)
        {
            byte[] retBytes;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<ThreadTraceResult>));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, traceList);
                retBytes = ms.ToArray();
            }
            

            return retBytes;
        }   
    }
}
