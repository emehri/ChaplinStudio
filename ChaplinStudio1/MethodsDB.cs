using System.Text;
namespace ChaplinStudio1
{
    public static partial class Database
    {
        public static void New()
        {
            Films.Clear();
            Author.Email = Author.Name = Author.Address = Author.Biography = Author.Phone = "";
        }
        public static string Save()
        {
            string ss = "";
            ss += "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n";
            ss += "<Chaplin>\r\n";
            string sp = "   ";
            Writers qq = Author;
            ss += $"{sp}<Writer Name=\"{qq.Name.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Email=\"{qq.Email.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Address=\"{qq.Address.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Biography=\"{qq.Biography.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Phone=\"{qq.Phone.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" />\r\n";
            foreach (var ww in Films)
            {
                ss += $"{sp}<Movie Note=\"{ww.Note.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Year=\"{ww.Year}\" BasedOn=\"{ww.BasedOn.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Title=\"{ww.Title.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\">\r\n";
                foreach (var ee in ww.Script)
                {
                    ss += $"{sp}{sp}<Act Note=\"{ee.Note.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Description=\"{ee.Description.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\">\r\n";
                    foreach (var rr in ee.Sequences)
                    {
                        ss += $"{sp}{sp}{sp}<Sequence Note=\"{rr.Note.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Description=\"{rr.Description.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" IsDay=\"{rr.IsDay}\" IsIn=\"{rr.IsIn}\" IsFade=\"{rr.IsFade}\" Location=\"{rr.Location.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\">\r\n";
                        foreach (var tt in rr.Happenings)
                        {
                            if (tt is Dialogue)
                            {
                                Dialogue yy = (Dialogue)tt;
                                ss += $"{sp}{sp}{sp}{sp}<Dialogue Note=\"{yy.Note.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Quote=\"{yy.Quote.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Manner=\"{yy.Manner.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Speaker=\"{yy.Speaker.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" />\r\n";
                            }
                            if (tt is Deed)
                            {
                                Deed yy = (Deed)tt;
                                ss += $"{sp}{sp}{sp}{sp}<Deed Note=\"{yy.Note.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" Deed=\"{yy.deed.Replace('"', '_').Replace('\n', ' ').Replace('\r', ' ')}\" />\r\n";
                            }
                        }
                        ss += $"{sp}{sp}{sp}</Sequence>\r\n";
                    }
                    ss += $"{sp}{sp}</Act>\r\n";
                }
                ss += $"{sp}</Movie>\r\n";
            }
            ss += "</Chaplin>\r\n";
            return ss;
        }
        public static string Load(string fileName)
        {
            List<string> line;
            try
            {
                line = File.ReadAllLines(fileName, Encoding.UTF8).ToList();
                if (line.Count == 0) { New(); return "File is empty."; }
            }
            catch
            {
                New();
                return "File Not Found.";
            }
            if (line == null) { New(); return "File is empty."; }
            Films.Clear();
            Writers qq = new Writers("", "", "", "", "");
            Movie ww = new Movie("", "", "", "", []);
            Part ee = new Part([], "", "");
            Seq rr = new Seq(false, false, false, "", "", [], "");
            for (int i = 0; i < line.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(line[i])) continue;
                string s = line[i].Trim();
                if (s.IndexOf("<Writer") == 0) Author = readWriter(s);

                if (s.IndexOf("<Movie") == 0) { ww = readMovie(s); ww.Script = new List<Part>(); }
                if (s.IndexOf("</Movie") == 0) Films.Add(ww);

                if (s.IndexOf("<Act") == 0) { ee = readAct(s); ee.Sequences = new List<Seq>(); }
                if (s.IndexOf("</Act") == 0) ww.Script.Add(ee);

                if (s.IndexOf("<Sequence") == 0) { rr = readSequence(s); rr.Happenings = new List<Element>(); }
                if (s.IndexOf("</Sequence") == 0) ee.Sequences.Add(rr);

                if (s.IndexOf("<Dialogue") == 0)
                {
                    var Gooyande = readDialogue(s);
                    rr.Happenings.Add(Gooyande);
                }
                if (s.IndexOf("<Deed") == 0) rr.Happenings.Add(readDeed(s));
            }
            return "";
        }
        public static void SeeActors()
        {
            foreach (var zz in Films)
            {
                zz.Actors.Clear();
                foreach (var xx in zz.Script)
                    foreach (var cc in xx.Sequences)
                        foreach (var vv in cc.Happenings)
                            if (vv is Dialogue dd)
                                try
                                {
                                    zz.Actors[dd.Speaker.ToUpper()]++;
                                }
                                catch
                                {
                                    zz.Actors[dd.Speaker.ToUpper()] = 1;
                                }
            }
        }
        static Writers readWriter(string s)
        {
            string q = "Name=\"";
            string Name = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Name += s[i];
            q = "Email=\"";
            string Email = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Email += s[i];
            q = "Address=\"";
            string Address = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Address += s[i];
            q = "Biography=\"";
            string Biography = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Biography += s[i];
            q = "Phone=\"";
            string Phone = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Phone += s[i];

            return new Writers(Name, Biography, Email, Phone, Address);
        }
        static Movie readMovie(string s)
        {
            string q = "Note=\"";
            string Note = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Note += s[i];
            q = "StoryLine=\"";
            string StoryLine = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                StoryLine += s[i];
            q = "Year=\"";
            string Year = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Year += s[i];
            q = "BasedOn=\"";
            string BasedOn = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                BasedOn += s[i];
            q = "Title=\"";
            string Title = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Title += s[i];

            return new Movie(Note, Year, Title, BasedOn, []);
        }
        static Part readAct(string s)
        {
            string q = "Note=\"";
            string Note = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Note += s[i];
            q = "Description=\"";
            string Description = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Description += s[i];

            return new Part([], Description, Note);
        }
        static Seq readSequence(string s)
        {
            string q = "Name=\"";
            q = "Note=\"";
            string Note = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Note += s[i];
            q = "Description=\"";
            string Description = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Description += s[i];
            q = "IsDay=\"";
            string IsDay = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                IsDay += s[i];
            q = "IsIn=\"";
            string IsIn = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                IsIn += s[i];
            q = "IsFade=\"";
            string IsFade = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                IsFade += s[i];
            q = "Location=\"";
            string Location = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Location += s[i];
            return new Seq(bool.Parse(IsDay), bool.Parse(IsIn), bool.Parse(IsFade), Description, Note, [], Location);
        }
        static Dialogue readDialogue(string s)
        {
            string q = "Name=\"";
            q = "Note=\"";
            string Note = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Note += s[i];
            q = "Quote=\"";
            string Quote = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Quote += s[i];
            q = "Manner=\"";
            string Manner = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Manner += s[i];
            q = "Speaker=\"";
            string Speaker = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Speaker += s[i];
            q = "IsFade=\"";
            Speaker = Speaker.ToUpper().Trim();
            return new Dialogue(Quote, Manner, Speaker, Note);
        }
        static Deed readDeed(string s)
        {
            string q = "Name=\"";
            q = "Note=\"";
            string Note = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                Note += s[i];
            q = "Deed=\"";
            string deed = "";
            for (int i = s.IndexOf(q) + q.Length; s[i] != '"'; i++)
                deed += s[i];
            return new Deed(deed, Note);
        }
        public static string Seq_HTML(Seq sequence)
        {
            if (sequence == null || sequence.Happenings == null || sequence.Happenings.Count == 0)
                return "";
            string ss = "<div style='margin:40px; font-size:12px; width:600px; font-size: 12px;'>";
            string IsFade = "DISSOLVE TO:";
            string IsDay = "DAY";
            string IsInt = "INT.";
            if (!sequence.IsFade) IsFade = "CUT TO:";
            if (!sequence.IsDay) IsDay = "NIGHT";
            if (!sequence.IsIn) IsInt = "EXT.";
            ss += $"<div style='text-align:right;'>{IsFade}</div>{IsInt} - {sequence.Location.ToUpper()} - {IsDay}<br><br>";
            foreach (var xx in sequence.Happenings)
            {
                if (xx is Dialogue)
                {
                    Dialogue dd = (Dialogue)xx;
                    string Gooyande = dd.Speaker;
                    if (string.IsNullOrWhiteSpace(Gooyande)) continue;
                    Gooyande = Gooyande.ToUpper().Trim() + "<br>";
                    string manner = dd.Manner;
                    if (!string.IsNullOrWhiteSpace(manner))
                    {
                        manner = manner.Replace("(", "").Replace(")", "").Trim();
                        manner = $"({manner})<br>";
                    }
                    ss += $"<div style='text-align:center; width:300px; margin-left:150px;'>";
                    ss += $"{Gooyande}<span style='width:250px;'>{manner}</span>{dd.Quote}</div><br>";
                }
                else
                {
                    Deed deed = (Deed)xx;
                    if (!string.IsNullOrWhiteSpace(deed.deed))
                        ss += "<div style='width:500px;margin-left:50px;'>" + deed.deed + "</div><br>";
                }
            }
            ss += "</div>";
            return ss;
        }
        public static string Export(Movie movie, string taraf)
        {
            if (movie == null || movie.Script == null || movie.Script.Count == 0) return "";
            string ss = "";
            string author_Name = "";
            string author_Biography = "";
            string author_Email = "";
            string author_Phone = "";
            string author_Address = "";
            string text_align = "left";
            bool valid = false;
            if (taraf == "rtl") text_align = "right";
            string style = "* {font-family: 'Courier New'; direction:" + taraf + "; } .Quote {text-align:center; width:300px;} td{margin-left: 20px;} table{margin-left:80px; margin-right:80px;} .deed1{width:500px; text-align:" + text_align + ";}";
            ss += "<html>\r\n<head><title>" + movie.Title + "</title><style>" + style + "</style></head>";
            ss += "<body><h1 style='text-align:center;'>" + movie.Title + "</h1><br><br><br>";
            if (!string.IsNullOrWhiteSpace(Author.Name)) author_Name = Author.Name + "<br>";
            if (!string.IsNullOrWhiteSpace(Author.Biography)) author_Biography = Author.Biography + "<br>";
            if (!string.IsNullOrWhiteSpace(Author.Email)) author_Email = Author.Email + "<br>";
            if (!string.IsNullOrWhiteSpace(Author.Phone)) author_Phone = Author.Phone + "<br>";
            if (!string.IsNullOrWhiteSpace(Author.Address)) author_Address = Author.Address + "<br>";
            ss += string.Format("<div style='text-align:center; margin:30px;'>{0}{1}{2}{3}{4}</div><hr>",
                author_Name, author_Biography, author_Email, author_Phone, author_Address);
            foreach (var act in movie.Script)
            {
                if (act == null || act.Sequences == null || act.Sequences.Count == 0) continue;
                foreach (var sequence in act.Sequences)
                {
                    string seq1 = Seq_HTML(sequence);
                    ss += seq1;
                    if (seq1 != "") valid = true;
                }
            }
            if (!valid) return "";
            ss += "</body></html>";
            return ss;
        }
    }
}
