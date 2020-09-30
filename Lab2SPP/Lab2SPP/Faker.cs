using GeneratorLibrary;
using System;
using System.Collections.Generic;

namespace Lab2SPP
{
    class Faker
    {
        private Dictionary<Type, Generator> _generators = new Dictionary<Type, Generator>();
        private Stack<Type> _generationStack;
        private ListGenerator _listGenerator;

        public Faker()
        {

        }
    }
}
