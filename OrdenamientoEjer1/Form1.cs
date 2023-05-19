using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace OrdenamientoEjer1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool estado = false; //Variable de control
        string[] Arreglos_datos; //Definimos un arreglo de texto, que contendra los datos a ordenar
        Button[] Arreglo; // Definimos un arreglo de botones, que nos ayudará para la simulación

        class Datos
        {
            private int longitud;           //Cantidad de datos a ordenar
            private string[] arreglo = new string[1];
            private Button[] arreglo_botones = new Button[1]; //nuevo arreglo de botones

            public Datos() //constructor clase 
            {
                string a = "";      //Variable auxiliar
                arreglo[0] = a; //texto que desplegará el botón
                arreglo_botones[0] = new Button();
                arreglo_botones[0].Width = 40;          //Definir ancho
                arreglo_botones[0].Height = 40;         //Definir Alto
                arreglo_botones[0].BackColor = Color.Blue; //Definir color del botón
                arreglo_botones[0].Text = a;
                Calcular_Longitud();
            }
            public void Calcular_Longitud()             //Método para saber cuantos datos se van a ordenar
            {
                longitud = arreglo.Length;
            }
            public int Obtener_Longitud()
            {
                return longitud;
            }
            public string[] Obtener_Arreglo()
            {
                return arreglo;
            }
            public void Insertar_Dato(string dato) //Función para insertar texto digitado por usuario como texto en botón
            {
                Array.Resize<string>(ref arreglo, longitud + 1); //se incrementará el arreglo en 1
                arreglo[longitud] = dato;
                Array.Resize<Button>(ref arreglo_botones, longitud + 1);
                arreglo[longitud] = dato;
                arreglo_botones[longitud] = new Button();
                arreglo_botones[0].Width = 90;          //Definir ancho
                arreglo_botones[0].Height = 50;         //Definir Alto
                arreglo_botones[0].BackColor = Color.GreenYellow; //Definir color del botón
                arreglo_botones[0].Text = dato;
                Calcular_Longitud();
            }

            public Button[] Arreglo_Botones()
            {
                return arreglo_botones;
            }
        }
        Datos Informacion = new Datos();      //Instanciamos la clase datos
        private char[] letraAbecedario = new char[] { 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f' };

        public void BubbleSort(ref string[] arreglo, ref Button[] Arreglo_Datos)
        {
            for (int i = 0; i < arreglo.Length; i++)
            {
                for (int j = 0; j < arreglo.Length - 1; j++)
                {
                    char[] palabra;
                    palabra = arreglo[i].ToCharArray();
                    if (palabra[0] == letraAbecedario[i])
                    {
                        Intercambio(ref Arreglo_Datos, j + 1, j);
                        string aux = arreglo[j];
                        arreglo[j] = arreglo[j + 1];
                        arreglo[j + 1] = aux;
                    }
                }
            }
        }
        public void Intercambio(ref Button[] boton, int a, int b)
        {
            string temp = boton[a].Text;    //Variable temporal

            Point pa = boton[a].Location;   //definimos la posición inicial
            Point pb = boton[b].Location;   //definimos posición final
            int diferencia = pa.X - pb.X;   //distacia a moverse en forma horizontal
            int x = 10;
            int y = 10;
            int t = 70;

            while (y != 70)
            {
                Thread.Sleep(t);
                /*Cantidad de movimientos a realizar cada botón en forma vertical
                para agregar pausas al hilo de la ejecución */
                boton[a].Location += new Size(0, 10);
                boton[b].Location += new Size(0, -10);
                y += 10;
            }
            while (x != diferencia + 10)     //Movimiento horizontal
            {
                Thread.Sleep(t);
                boton[a].Location += new Size(-10, 0);
                boton[b].Location -= new Size(10, 0);
                x += 10;
            }

            y = 0;

            while (y != -60)                //Movimiento vertical
            {
                Thread.Sleep(t);
                boton[a].Location += new Size(0, -10);
                boton[b].Location += new Size(0, +10);
                y -= 10;
            }

            boton[a].Text = boton[b].Text;
            boton[b].Text = temp;
            boton[b].Location = pb;
            boton[a].Location = pa;
            estado = true;
            tabPage1.Refresh();
        }
        private void TxtDatos_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string letras = txtDatos.Text;
                Informacion.Insertar_Dato(letras);                   //se agrega objeto a "Información"
                Arreglos_datos = Informacion.Obtener_Arreglo();      //se sacan los  arreglos del objeto "Datos"
                Arreglo = Informacion.Arreglo_Botones();
            }
            catch
            {
                MessageBox.Show("Solo se admiten números enteros");
            }
            estado = true; //Cambia el valor de variable de control de simulación
            tabPage1.Refresh();
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            if (estado)
            {
                Point xy = new Point(50, 70);   //método de librería Drawing
                try
                {
                    Dibujar_Arreglo(ref Arreglo, xy, ref tabPage1);
                    //Función que dibujará los datos en la simulación
                }
                catch
                { }
                estado = false;
            }
        }
        public void Dibujar_Arreglo(ref Button[] Arreglo, Point xy, ref TabPage t)
        {
            for (int i = 1; i < Arreglo.Length; i++)
            {
                Arreglo[i].Location = xy;
                t.Controls.Add(Arreglo[i]);
                xy += new Size(70, 0);
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;   //Cambiamos la apariencia del cursor a modo espera

            btnOrdenar.Enabled = false;
            txtDatos.Enabled = false;
            btnAgregar.Enabled = false;

            BubbleSort(ref Arreglos_datos, ref Arreglo);

            this.Cursor = Cursors.Default;  //retorna la apariencia del cursor a uno por defecto

            btnOrdenar.Enabled = true;
            txtDatos.Enabled = true;
            btnAgregar.Enabled = true;
        }

        static void Ordenamiento_Insercion(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int temp = array[i];
                int j = i - 1;
                while ((j >= 0) && (array[j] > temp))
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = temp;
            }
        }
    }
}