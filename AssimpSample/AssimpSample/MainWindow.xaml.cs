using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;
using System.Globalization;

namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();

            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World((int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight, openGLControl.OpenGL);
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.Width, (int)openGLControl.Height);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {

                case Key.Add:
                    if (!m_world.AnimationActive)
                        m_world.SceneDistance -= 700.0f; break;
                case Key.Subtract:
                    if (!m_world.AnimationActive)
                        m_world.SceneDistance += 700.0f; break;
                case Key.F4:
                    if (!m_world.AnimationActive)
                        this.Close(); break;
                case Key.I:
                    if (!m_world.AnimationActive)
                        m_world.RotationX += 5.0f; break;
                case Key.K:
                    if (!m_world.AnimationActive)
                        m_world.RotationX -= 5.0f; break;
                case Key.J:
                    if (!m_world.AnimationActive)
                        m_world.RotationY -= 5.0f; break;
                case Key.L:
                    if (!m_world.AnimationActive)
                        m_world.RotationY += 5.0f; break;


                case Key.V:
                    {
                        m_world.RefreshScene();
                        m_world.ActivateAnimation(); break;
                    }




            }
        }

        private void transliranjeLevogBolida_TextChanged(object sender, TextChangedEventArgs e)
        {
            float i;
            if (transliranjeLevogBolida.Text.Length <= 0) return;

            if (m_world != null && float.TryParse(transliranjeLevogBolida.Text, out i) && !m_world.AnimationActive)
                m_world.TransliranjeLevogBolida = float.Parse(transliranjeLevogBolida.Text, CultureInfo.InvariantCulture.NumberFormat);
        }

        private void rotacijaDesnogBolida_TextChanged(object sender, TextChangedEventArgs e)
        {
            float i;
            if (rotacijaDesnogBolida.Text.Length <= 0) return;
            if (float.TryParse(rotacijaDesnogBolida.Text, out i) && m_world != null && !m_world.AnimationActive)
               m_world.RotacijaDesnogBolida = float.Parse(rotacijaDesnogBolida.Text, CultureInfo.InvariantCulture.NumberFormat);
        }

        private void faktorSkaliranjaBolida_TextChanged(object sender, TextChangedEventArgs e)
        {
            float i;
            if (faktorSkaliranjaBolida.Text.Length <= 0) return;
            if (float.TryParse(faktorSkaliranjaBolida.Text, out i) && m_world != null && !m_world.AnimationActive)
            {
                m_world.SkaliranjeBolida1 = float.Parse(faktorSkaliranjaBolida.Text, CultureInfo.InvariantCulture.NumberFormat);
           
            }
              
        }

        private void faktorSkaliranjaDesnogBolida_TextChanged(object sender, TextChangedEventArgs e)
        {
            float i;
            if (faktorSkaliranjaDesnogBolida.Text.Length <= 0) return;
            if (float.TryParse(faktorSkaliranjaDesnogBolida.Text, out i) && m_world != null && !m_world.AnimationActive)
            {
                m_world.SkaliranjeBolida2 = float.Parse(faktorSkaliranjaDesnogBolida.Text, CultureInfo.InvariantCulture.NumberFormat);
               
            }
        }
    }
}
