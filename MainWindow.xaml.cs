using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Net;
using System.Net.Mail;

namespace WpfApp11
{
    public partial class MainWindow : Window
    {
        Workbook wb;
        Worksheet sheet;
        CellRange locatedRange;
        DataTable dataTable;

        public MainWindow()
        {
            InitializeComponent();
            InitializeExcel();
        }

        private void UpdateGrid()
        {
            locatedRange = sheet.AllocatedRange; // Обновить диапазон
            dataTable = sheet.ExportDataTable(locatedRange, true);
            grid.ItemsSource = dataTable.DefaultView;
        }

        private void InitializeExcel()
        {
            wb = new Workbook();
            sheet = wb.Worksheets.Add("Лист 1");
            locatedRange = sheet.AllocatedRange;
            dataTable = sheet.ExportDataTable(locatedRange, true);
            grid.ItemsSource = dataTable.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sheet.InsertRow(1, 1);
            UpdateGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItems != null)
            {
                List<DataRowView> rowsToDelete = new List<DataRowView>();

                foreach (var selectedItem in grid.SelectedItems)
                {
                    DataRowView row = selectedItem as DataRowView;
                    if (row != null && !IsRowEmpty(row))
                    {
                        int rowIndex = dataTable.Rows.IndexOf(row.Row);
                        sheet.DeleteRow(rowIndex + 1);
                        rowsToDelete.Add(row);
                    }
                }

                foreach (var row in rowsToDelete)
                {
                    dataTable.Rows.Remove(row.Row);
                }

                UpdateGrid();
            }
        }

        private bool IsRowEmpty(DataRowView row)
        {
            foreach (var item in row.Row.ItemArray)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    sheet.InsertDataTable(dataTable, true, 1, 1); 

                    wb.SaveToFile(saveFileDialog.FileName, FileFormat.Version2013);

                    MessageBox.Show("Файл сохранен успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            InitializeExcel();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    wb.LoadFromFile(openFileDialog.FileName);
                    sheet = wb.Worksheets[0]; // Загрузить первый лист
                    UpdateGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    sheet.InsertDataTable(dataTable, true, 1, 1);

                    wb.SaveToFile(saveFileDialog.FileName, FileFormat.Version2013);

                    string from = FromTextBox.Text;
                    string to = ToTextBox.Text;
                    string subject = SubjectTextBox.Text;
                    string body = BodyTextBox.Text;

                    MailMessage mail = new MailMessage(from, to, subject, body);
                    Attachment attachment = new Attachment(saveFileDialog.FileName);
                    mail.Attachments.Add(attachment);

                    SmtpClient client = new SmtpClient("smtp.your-email-provider.com", 587);
                    client.Credentials = new NetworkCredential("your-email@example.com", "your-password");
                    client.EnableSsl = true;
                    client.Send(mail);

                    MessageBox.Show("Письмо успешно отправлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при отправке письма: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
