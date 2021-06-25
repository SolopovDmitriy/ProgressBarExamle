using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProgressBarExamle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();//инициализация компонентов
            
          
        }


        void SimpleMethod()// объявление метода
        {            
            for (int i = 0; i < 100; i++)
            {
                Dispatcher.Invoke(//способ получить доступ к прогресс бару
                     new System.Action(() => { 
                         pbStatus.Value++;// увеличивает значение на условную единицу progressbar
                         // TextBoxProgress.Text = " Прогресс: " + i + "%";
                     })
                );

                Thread.Sleep(500);// каждую итерацию цикла ждем полсекунды
            }
        }


        //void openBigTextFile(object fileName)// открывает текстовый документ
        //{
        //    string s = File.ReadAllText(fileName.ToString()); // чтение файла
        //    Dispatcher.Invoke(//способ получить доступ к txtEditor
        //            new System.Action(() =>
        //            {
        //                txtEditor.Text = s;
        //            })
        //       );
        //}


        void openBigTextFile(object fileName)// открывает текстовый документ
        {
            int i = 0;
            string allText = "";
            int lineCount = File.ReadLines(fileName.ToString()).Count(); // количество строк в файле
            
            using (var fileStream = File.OpenRead(fileName.ToString())) //  - открытие файлового потока fileStream
            using (var streamReader = new StreamReader(fileStream)) // StreamReader - объект, который читает файл
            {               
                String line;
                while ((line = streamReader.ReadLine()) != null) // чтение файла построчно
                {
                    i++; // номер текущей строки
                    allText += line; // строку, которую прочитали из файла, прибавляем к строке 
                    if (i % 100 == 0) // выполняется когда i = 100, 200, 300, ...
                    {
                        Dispatcher.Invoke(//способ получить доступ к txtEditor
                            new System.Action(() =>
                            {
                                int percent = (int)Math.Round(i * 100.0 / lineCount, MidpointRounding.AwayFromZero);
                                pbStatus.Value = percent; // прогресс бар 
                                TextBoxProgress.Text = "  Прогресс: " +  percent + "%"; // печатает проценты
                            })
                        );
                    }
                }

            }


            Dispatcher.Invoke(//способ получить доступ к txtEditor
                           new System.Action(() =>
                           {
                               txtEditor.Text = allText;
                           })
                       );
           


        }





        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //создание потока, который имитирует окрытие большого файла
            //ThreadStart nextStart = new ThreadStart(SimpleMethod);
            //Thread nextone = new Thread(nextStart);
            //nextone.Priority = ThreadPriority.Lowest;
            //nextone.Start();
            //nextone.IsBackground = true;//делаем поток фоновым

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();// создаем диалоговое окно для открытия файла
            if (openFileDialog.ShowDialog() == true)// показываем диалоговое окно
            {
                ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(openBigTextFile); // объект, который хранит метод, который будет выполнятся в потоке
                Thread thirdThread = new Thread(parameterizedThreadStart);// создаем третий поток
                thirdThread.Start(openFileDialog.FileName);// запускаем третий поток
                
                // openBigTextFile(openFileDialog.FileName);// открываем при нажатии на кнопку текстовый документ
            }
        }

    }
}



//    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
//    {
//        this.pbStatus.Value++; // Do all the ui thread updates here
//    }));