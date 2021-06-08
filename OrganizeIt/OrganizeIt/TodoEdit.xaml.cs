﻿using OrganizeIt.backend.todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrganizeIt.backend
{
    /// <summary>
    /// Interaction logic for TodoEdit.xaml
    /// </summary>
    public partial class TodoEdit : Window
    {
        public TodoEdit()
        {
            InitializeComponent();
        }

        public TodoEdit(ToDoCard card)
        {
            InitializeComponent();
            this.DataContext = card;
        }

        public ToDoCard Answer
        {
            get
            {
                return this.DataContext as ToDoCard;
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            string status = (StatusBox.SelectedItem as ComboBoxItem).Content as string;
            var card = this.DataContext as ToDoCard;

            ToDoStatus newStatus;

            switch (status)
            {
                case "Novo":
                    newStatus = ToDoStatus.ToDo;
                    break;
                case "U obradi":
                    newStatus = ToDoStatus.Processing;
                    break;
                case "Poslato":
                    newStatus = ToDoStatus.Sent;
                    break;
                case "Prihvaceno":
                    newStatus = ToDoStatus.Accepted;
                    break;
                case "Odbijeno":
                    newStatus = ToDoStatus.Rejected;
                    break;
                case "ARHIVIRAJ":
                    string messageBoxText = $"Da li zelite da obrisete zadatak";
                    string caption = "Brisanje";
                    MessageBoxButton btn = MessageBoxButton.YesNo;
                    MessageBoxImage img = MessageBoxImage.Question;

                    var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
                    if (result == MessageBoxResult.No)
                        return;

                    newStatus = ToDoStatus.Deleted;
                    break;
                default:
                    newStatus = card.Status;
                    break;
            }

            string name = NameBox.Text;
            string description = DescriptionBox.Text;
            card.Name = name;
            card.Description = description;
            card.Status = newStatus;
            this.DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}