using System;
using System.Collections.Generic;
using System.IO;

namespace Audio_Manip
{
    public class Format
    {
        public enum States
        {
            bank,
            npck,
            nbnk,
            wem,
            ogg,
            channel,
        }

        private readonly States _state;
        private static readonly Dictionary<States, States> Transitions = new Dictionary<States, States>
        {
            {States.bank,States.wem},
            {States.nbnk,States.wem},
            {States.npck,States.wem},
            {States.wem,States.ogg},
            {States.ogg,States.channel}
        };
        private static readonly Dictionary<String, States> Mapping = new Dictionary<String, States>
        {
            {"bank",States.bank},
            {"npck",States.npck},
            {"nbnk",States.nbnk},
            {"wem",States.wem},
            {"ogg",States.ogg},
            {"channel",States.channel}
        };

        public Format(States extension = States.bank)
        {
            _state = extension;
        }
        
        //NOTE - LowerLevel works at level basis, this works for the time because the graph transitions are unique.
        public Format LowerLevel()
        {
            if (!Transitions.TryGetValue(_state, out States next))
                throw new Exception("Cannot lower level further than " + _state.ToString());
            return new Format(next);
        }

        public void Unpack(string file, string destination)
        {
            switch (_state)
            {
                case States.nbnk:
                    break;
                case States.npck:
                    break;
                case States.wem:
                    break;
                case States.ogg:
                    break;
                default:
                    throw new Exception("No unpacking function exists for " + _state.ToString());
            }
            //TODO - Selectively call each of the relevant unpacks
            //TODO - Code unpacking banks
            //TODO - Code unpacking wem (Raviolli)
            //TODO - Code unpacking channels from an ogg
            //More switches than nintendo, more cases than an apple
        }

        public bool Unpackable()
        {
            return _state!=States.channel;
        }

        //NOTE - Recognize works at level basis instead of at format basis, since we have a relatively simple structure we can
        //treat both in the same class but splitting it to Levels might be required at some point, allowing Formats to
        //get their own level.
        
        public bool Recognize(string filename)
        {
            return StateFamily(_state, Guess(filename)._state);
        }
        
        static public Format Guess(string filename)
        {
            if (!Mapping.TryGetValue(Path.GetExtension(filename), out States state))
            {
                throw new Exception("File extension not recognised" + Path.GetExtension(filename));
            }
            if (state==States.ogg)
            {
                //TODO - Finish handling the difference between an ogg and it's channels.
                state = States.ogg;
            } 
            return new Format(state);
        }

        //HACK - There's no clean way of doing this since it's a non transitive equality which would be better drawn
        // in a class diagram, alas that's making classes all the way to the bottom
         public bool StateFamily(States left, States right)
        {
            if (left != States.bank && right != States.bank) return left == right;
            return (left == States.nbnk || left == States.npck || right == States.nbnk || right == States.npck);
        }
    }
}