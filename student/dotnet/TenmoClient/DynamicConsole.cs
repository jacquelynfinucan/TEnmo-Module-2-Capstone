using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenmoClient
{
    public class DynamicConsole
    {
        private List<ConsoleItem> listOfItems = new List<ConsoleItem>();

        public int Length { get { return listOfItems.Count(); } }
        public ConsoleItem this[int i]
        {
            get
            {
            return listOfItems[i]; 
            }
        }

        /// <summary>
        /// Adds lines of strings to the end of the console.
        /// </summary>
        /// <param name="strings"></param>
        public void Add(params string[] strings)
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0;i<strings.Length;i++)
            {
                Console.SetCursorPosition(0, listOfItems.Count);
                Console.WriteLine(strings[i]);
                listOfItems.Add(new ConsoleItem(strings[i]));
                listOfItems[listOfItems.Count - 1].ItemIndex = listOfItems.Count - 1;
            }
        }
        /// <summary>
        /// Removes a number of lines from the end of console.
        /// </summary>
        /// <param name="linesToRemove"></param>
        public void Remove(int linesToRemove)
        {
            while (linesToRemove > 0)
            {
                listOfItems[listOfItems.Count - 1].Remove();
                listOfItems.RemoveAt(listOfItems.Count - 1);
                linesToRemove--;
            }
        }

        public void Clear()
        {
            while (listOfItems.Count>0)
            {
                listOfItems[listOfItems.Count - 1].Remove();
                listOfItems.RemoveAt(listOfItems.Count - 1);
            }
        }

        public string ReadLine()
        {
            var var = Console.CursorTop;
            var str = Console.ReadLine();
            Console.SetCursorPosition(0, var);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            return str;
        }

        public class ConsoleItem
        {
            
            public string Text { get; set; }

            public int ItemIndex { get; set; }

            public ConsoleItem(string text)
            {
                this.Text = text;
            }

            /// <summary>
            /// Changes the color of the line in the console.
            /// </summary>
            /// <param name="color"></param>

            public async Task ChangeColor(ConsoleColor color,bool slow = false)
            {
                
                Console.SetCursorPosition(0, ItemIndex);
                Console.ForegroundColor = color;

                if (!slow)
                {
                    Console.WriteLine(Text);
                } else
                {
                    for(int i = 0; i <Text.Length; i++)
                    {
                        Console.Write(Text[i]);
                        await Task.Delay(12);
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
            }

            public async Task ChangeColorPrecise(ConsoleColor color, bool slow = true)
            {

                Console.SetCursorPosition(0, ItemIndex);
                Console.ForegroundColor = color;

                if (!slow)
                {
                    Console.WriteLine(Text);
                }
                else
                {
                    for (int i = 0; i < Text.Length; i++)
                    {

                        Console.SetCursorPosition(i, ItemIndex);
                        Console.ForegroundColor = color;
                        Console.Write(Text[i]);
                        await Task.Delay(150);
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
            }

            public async Task DoARainbow()
            {
                Console.CursorVisible = false;
                while (true)
                {
                    this.ChangeColorPrecise(ConsoleColor.Red, true);
                    await Task.Delay(800);
                    this.ChangeColorPrecise(ConsoleColor.Yellow, true);
                    await Task.Delay(800);
                    this.ChangeColorPrecise(ConsoleColor.Green, true);
                    await Task.Delay(800);
                    this.ChangeColorPrecise(ConsoleColor.Magenta, true);
                    await Task.Delay(800);
                    this.ChangeColorPrecise(ConsoleColor.Blue, true);
                    await Task.Delay(800);
                }
                Console.CursorVisible = true;
            }

            public void Remove()
            {
                Console.SetCursorPosition(0, ItemIndex);
                Console.WriteLine(new string(' ',Console.BufferWidth));
            }


        }
    }
}
