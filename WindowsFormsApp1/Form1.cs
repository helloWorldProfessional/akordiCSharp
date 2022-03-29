using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class Chords : Form
    {
        public Chords()
        {
            InitializeComponent();
        }
        string chord;
        string guitarS = "e B G D A E";
        private void Chords_Load(object sender, EventArgs e)
        {
            string[] rootFill = { "E", "A", "D", "G", "B", "C", "F" };
            string[] qualityFill = { "sus2", "sus4", "maj7", "m", "dim","b"};
            string[] tensionFill = { "5", "6", "7", "7b5"};

            for (int i = 0; i < rootFill.Length; i++)
            {
                rootCB.Items.Add(rootFill[i].ToString());
            }
            for (int i = 0; i < qualityFill.Length; i++)
            {
                qualityCB.Items.Add(qualityFill[i].ToString());
            }
            for (int i = 0; i < tensionFill.Length; i++)
            {
                tensionCB.Items.Add(tensionFill[i].ToString());
            }

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();

                if (String.IsNullOrEmpty(qualityCB.GetItemText(qualityCB.SelectedItem)))
                {
                    if (String.IsNullOrEmpty(tensionCB.GetItemText(tensionCB.SelectedItem)))
                        chord = rootCB.GetItemText(rootCB.SelectedItem);
                    else
                        chord = rootCB.GetItemText(rootCB.SelectedItem) + "_" + tensionCB.GetItemText(tensionCB.SelectedItem);
                }
                else if (qualityCB.GetItemText(qualityCB.SelectedItem) == "b")
                {
                        if (String.IsNullOrEmpty(tensionCB.GetItemText(tensionCB.SelectedItem)))
                            chord = rootCB.GetItemText(rootCB.SelectedItem) + qualityCB.GetItemText(qualityCB.SelectedItem);
                        else
                            chord = rootCB.GetItemText(rootCB.SelectedItem) + qualityCB.GetItemText(qualityCB.SelectedItem)+ tensionCB.GetItemText(tensionCB.SelectedItem);
                }
                else{
                        chord = rootCB.GetItemText(rootCB.SelectedItem) + "_" + qualityCB.GetItemText(qualityCB.SelectedItem) + tensionCB.GetItemText(tensionCB.SelectedItem);
                }

            string apiUrl = String.Format("https://api.uberchord.com/v1/chords/" + chord);
            WebClient client = new WebClient();
            string fResponse = client.DownloadString(apiUrl);
            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(fResponse);
            ChordClass res = new ChordClass();
            foreach (var i in dobj)
            {
                res.Strings = i["strings"];
                res.Fingering = i["fingering"];
                res.ChordName = i["chordName"];
                res.EnharmonicChordName = i["enharmonicChordName"];
                res.VoicingId = i["voicingID"];
                res.Tones = i["tones"];
               
            }
            chordEntered.Text = chord;
            chordTones.Text = res.Tones;
            char[] chordFin;
            char[] chordPattern;
            chordFin = res.Strings.ToCharArray();
            Array.Reverse(chordFin);
            chordPattern = res.Fingering.ToCharArray();
            Array.Reverse(chordPattern);

            for (int i = 0; i <= 10; i++)
            {
                listBox1.Items.Add("  " + guitarS[i] + " || " + chordFin[i] + " ");
                listBox2.Items.Add("  " + guitarS[i] + " || " + chordPattern[i] + " ");
            }
        }catch (Exception error) {
                MessageBox.Show(chord+" akord nije pronađen!");};
        }
    }
    public class ChordClass
    {
        [JsonProperty("strings")]
        public string Strings { get; set; }

        [JsonProperty("fingering")]
        public string Fingering { get; set; }

        [JsonProperty("chordName")]
        public string ChordName { get; set; }

        [JsonProperty("enharmonicChordName")]
        public string EnharmonicChordName { get; set; }

        [JsonProperty("voicingID")]
        public string VoicingId { get; set; }

        [JsonProperty("tones")]
        public string Tones { get; set; }
    }

}







