using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaplinStudio1
{
    public static partial class Database
    {
        public static List<Movie> Films = new List<Movie>();
        public static Writers Author = new Writers("", "", "", "", "");
    }
    public enum WhatIsIt
    {
        Nothing,
        Movie,
        Act,
        Sequence,
        Character
    }

    public class Writers
    {
        public string Name { get; set; } = "";
        public string Biography { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public Writers(string name, string biography, string email, string phone, string address)
        {
            Name = name;
            Biography = biography;
            Email = email;
            Phone = phone;
            Address = address;
        }
    }
    public class Seq
    {
        public bool IsDay { get; set; } = false;
        public bool IsIn { get; set; } = false;
        public string Description { get; set; } = "";
        public string Note { get; set; } = "";
        public List<Element> Happenings = new List<Element>();
        public bool IsFade { get; set; } = false;
        public string Location { get; set; } = "";
        public Seq(bool isDay, bool isIn, bool isFade, string description, string note, List<Element> happenings, string location)
        {
            IsDay = isDay;
            IsIn = isIn;
            IsFade = isFade;
            Description = description;
            Note = note;
            Happenings = happenings;
            Location = location;
        }
    }
    public class Movie
    {
        public string Note { get; set; } = "";
        public string Year { get; set; } = "";
        public string Title { get; set; } = "";
        public string BasedOn { get; set; } = "";
        public List<Part> Script = new List<Part>();
        public Dictionary<string, int> Actors = new Dictionary<string, int>();
        public Movie(string note, string year, string title, string basedOn, List<Part> script)
        {
            Note = note;
            Year = year;
            Title = title;
            BasedOn = basedOn;
            Script = script;
        }
    }
    public class Element
    {
        public string Note { get; set; } = "";
        public bool isBeingEdited { get; set; } = false;
    }
    public class Dialogue : Element
    {
        public string Quote { get; set; } = "";
        public string Manner { get; set; } = "";
        public string Speaker { get; set; } = "";
        public Dialogue(string quote, string manner, string speaker, string Note)
        {
            Quote = quote;
            Manner = manner;
            Speaker = speaker;
            this.Note = Note;
        }
    }
    public class Deed : Element
    {
        public string deed { get; set; } = "";
        public Deed(string deed, string Note)
        {
            this.deed = deed;
            this.Note = Note;
        }
    }
    public class Part
    {
        public List<Seq> Sequences = new List<Seq>();
        public string Note { get; set; } = "";
        public string Description { get; set; } = "";
        public Part(List<Seq> sequences, string description, string note)
        {
            Sequences = sequences;
            Description = description;
            Note = note;
        }
    }
}
