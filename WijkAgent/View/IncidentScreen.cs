﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using WijkAgent.Model;

namespace WijkAgent
{
    public partial class IncidentScreen : Form
    {
        SQLConnection sql = new SQLConnection();
        List<CheckBox> TwitterCheckboxes = new List<CheckBox>();

        // zodat alles netjes onderelkaar komt
        private int top = 20;

        private Font labelFont;

        #region Constructor
        public IncidentScreen(int _districtId)
        {
            InitializeComponent();

            labelFont = new Font("Calibri", 12, FontStyle.Bold);

            // alle categorien en twitter berichten ophalen
            Dictionary<int, string> categories = sql.GetAllCategory();
            Dictionary<int, string> twitterMessages = sql.GetAllTwitterMessageFromDistrictToday(_districtId);

            // zodat je iet kan zien en sluiten + scrollbar
            twitterIncidentPanel.AutoScroll = true;
            this.ControlBox = false;

            // klik functies doorsturen naar andere methodes
            cancelIncidentButton.Click += CancelButtonClick;
            selectAllCheck.CheckStateChanged += SelectAllCheckClicked;
            saveIncidentButton.Click += saveIncident;

            foreach (KeyValuePair<int, string> entry in categories)
            {
                // toevoegen aan category, combobox met hoofletter
                categoryCombo.Items.Add(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(entry.Value));
            }

            // controlleren of er berichten zijn die opgeslagen kunnen worden.
            if (twitterMessages.Count == 0)
            {
                Label message = new Label() { Font = labelFont, Text = "Geen onopgelsagen tweets, binnen 24 uur, gevonden bij deze wijk.", Top = this.top, AutoSize = true, MaximumSize = new Size(300, 0) };
                twitterIncidentPanel.Controls.Add(message);
                message.Dock = DockStyle.Fill;

                // opslaan knop verbergen
                saveIncidentButton.Hide();
                cancelIncidentButton.Location = new Point(440, 518);
            }
            else
            {
                foreach (KeyValuePair<int, string> entry in twitterMessages)
                {
                    CheckBox checkMessage = new CheckBox() { Left = this.Left + 31, Top = this.top, AutoSize = true, Name = entry.Key.ToString() };
                    Label twitterMessage = new Label() { Font = labelFont, Text = entry.Value, Top = this.top, AutoSize = true, MaximumSize = new Size(300, 0), Left = checkMessage.Width, Name = entry.Key.ToString() };

                    twitterIncidentPanel.Controls.Add(checkMessage);
                    TwitterCheckboxes.Add(checkMessage);
                    twitterIncidentPanel.Controls.Add(twitterMessage);

                    this.top = this.top + 20 + twitterMessage.Height;
                }
            }
        }
        #endregion

        #region Save_Incidents_Method
        private void saveIncident(object sender, EventArgs e)
        {
            if (categoryCombo.SelectedIndex > -1)
            {
                // geselecteerde categorie ophalen
                string _selectedCategory = categoryCombo.SelectedItem.ToString();
                List<CheckBox> _selectedTwitterCheckboxes = new List<CheckBox>();

                foreach (CheckBox box in TwitterCheckboxes)
                {
                    if (box.Checked)
                    {
                        // voeg toe aan de list
                        _selectedTwitterCheckboxes.Add(box);
                    }
                }
                if (_selectedTwitterCheckboxes.Count > 0)
                {
                    // database update
                    foreach (CheckBox box in _selectedTwitterCheckboxes)
                    {
                        // de naam van elke checkbox heeft het bericht id, 
                        // dit is een string maar moet omgezet worden naar een int
                        sql.updateTwitterMessageCategory(Int32.Parse(box.Name), _selectedCategory);
                    }
                    MessageBox.Show("Voorval toegevoegd");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kies tenminste 1 bericht die moet worden gecombineerd aan het gewenste categorie");
                }
            }
            else
            {
                MessageBox.Show("Kies een categorie");
            }
        }
        #endregion

        #region Select_All_Tweets_CheckBox_Click
        // als die verandert, kijk of die check of unchecked is,
        // dan de rest checken of unchecken
        private void SelectAllCheckClicked(object sender, EventArgs e)
        {
            if (selectAllCheck.Checked)
            {
                foreach (CheckBox box in TwitterCheckboxes)
                {
                    box.Checked = true;
                }
            }
            else if (!selectAllCheck.Checked)
            {
                foreach (CheckBox box in TwitterCheckboxes)
                {
                    box.Checked = false;
                }
            }
        }
        #endregion

        #region Incidents_Cancel_Button_Click
        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}
