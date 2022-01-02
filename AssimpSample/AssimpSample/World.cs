// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Threading;
using SharpGL;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi
        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene_firstCar;

        private AssimpScene m_scene_secondCar;
        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;
        private float transliranjeLevogBolida = 0.0f;
        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;
        public OpenGL gl;
        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 7000.0f;

        private float rotacijaDesnogBolida = 0.0f;
        private float skaliranjeBolida1 = 250.35f;
        private float skaliranjeBolida2 = 0.35f;
        private float translatePoZ = -5000f;
        private float translatePoZBolid2 = -4900f;
        public int zoomInCounter = 0;
        private int zoomOutCounter = 0;
        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;
        private float eyeZ = 80.0f;
        private float eyeY = 30.0f;
        private float eyeX = 0.0f;
        private float centerZ = -15.0f;

        private bool animationActive = false;

        private DispatcherTimer timer1;
        private DispatcherTimer timer2;
        private DispatcherTimer timer3;
        private DispatcherTimer timer4;
        private DispatcherTimer timer5;
        private DispatcherTimer timer6;

        private enum Textures { Asfalt = 0, Metal = 1, Sljunak = 2 };
        private uint[] m_textures = null;
        private string[] m_textureImages = { "..//..//Textures//asfalt.jpg", "..//..//Textures//metal.jpg", "..//..//Textures//Sljunak.jpg" };

        private int m_textureCount = Enum.GetNames(typeof(Textures)).Length;


        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene_secondCar; }
            set { m_scene_secondCar = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { if (value > 0 && value < 90) m_xRotation = value; }
        }


        public bool AnimationActive
        {
            get
            {
                return animationActive;
            }
            set
            {
                animationActive = value;
              
            }
        }

        public float TransliranjeLevogBolida
        {
            get { return transliranjeLevogBolida; }
            set { transliranjeLevogBolida = value; }
        }

        public float SkaliranjeBolida1
        {
            get { return skaliranjeBolida1; }
            set { skaliranjeBolida1 = value; }
        }

        public float SkaliranjeBolida2
        {
            get { return skaliranjeBolida2; }
            set { skaliranjeBolida2 = value; }
        }


        public float RotacijaDesnogBolida
        {
            get { return rotacijaDesnogBolida; }
            set { rotacijaDesnogBolida = value; }
        }
        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }
        /// <summary>
        ///	 X pozicija kamere.
        /// </summary>
        public float EyeX
        {
            get { return eyeX; }
            set { eyeX = value; }
        }

        /// <summary>
        ///	 Y pozicija kamere.
        /// </summary>
        public float EyeY
        {
            get { return eyeY; }
            set { eyeY = value; }
        }

        /// <summary>
        ///	 Z pozicija kamere.
        /// </summary>
        public float EyeZ
        {
            get { return eyeZ; }
            set { if (value > 1) eyeZ = value; }
        }

        public float CenterZ
        {
            get { return centerZ; }
            set { centerZ = value; }
        }


        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(int width, int height, OpenGL gl)
        {
            this.gl = gl;
            this.m_scene_firstCar = new AssimpScene("3D Models\\Bolid1", "Bolid1.3ds", gl);
            this.m_scene_secondCar = new AssimpScene("3D Models\\Car2", "Car tasergal xform N200214.3DS", gl);
            // this.m_scene_secondCar = new AssimpScene("3D Models\\castle2", "castle.obj", gl);

            this.m_width = width;
            this.m_height = height;

            m_textures = new uint[m_textureCount];

        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);

            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.Enable(OpenGL.GL_NORMALIZE);

            TopLighting();
            DefineTimers();
            Tekstura(gl);


            m_scene_secondCar.LoadScene();
            m_scene_secondCar.Initialize();

            m_scene_firstCar.LoadScene();
            m_scene_firstCar.Initialize();
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>




        public void Tekstura(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D); // omogucavanje upotrebe tekstura
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE); //način stapanja teksture sa materijalom 
            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);

                Bitmap image = new Bitmap(m_textureImages[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData imageData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0);

                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR); //filteri za teksture
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR); //linearno filtriranje
                //wrapping GL_REPEAT po obema osama
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

                // Posto je kreirana tekstura slika nam vise ne treba
                image.UnlockBits(imageData);
                image.Dispose();
            }
        }
        private void TopLighting()
        {


            gl.Enable(OpenGL.GL_LIGHTING);         // ukljucujemo osvetljenje
            gl.Enable(OpenGL.GL_LIGHT0);           // svetlo0 koje se koristi za tackasti izvor



            float[] ambientColor1 = { 0.4f, 0.4f, 0.4f, 1.0f };
            float[] diffuseColor = { 1.0f, 1.0f, 1.0f, 1.0f };  // bela boja
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambientColor1);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, diffuseColor);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f); // tackasti izvor
            //gore u centar scene 
            float[] lightPosition1 = { 0.0f, 2.0f, 0.0f, 1.0f };
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, lightPosition1);



            gl.Enable(OpenGL.GL_LIGHT1);// svetlo1 koje se koristi kao reflektor zute boje
            float[] whiteColor = { 1.0f, 1.0f, 0.0f, 1.0f };
            float[] ambientColor2 = { 0.4f, 0.4f, 0.4f, 1.0f };
            float[] lightPosition2 = { -730f, -2000f, -3300f };
            float[] light_direction = { 0.0f, -1.0f, 0.0f, 1.0f };
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambientColor2);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, whiteColor);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, lightPosition2);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, light_direction);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 45.0f); // cut-off=45


        }
        public void Draw(OpenGL gl)
        {

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.PushMatrix();
            gl.Translate(0.0f, 0.0f, -m_sceneDistance);
            gl.LookAt(eyeX, eyeY, eyeZ, 0.0f, 0.0f, centerZ, 0.0f, 1.0f, 0.0f);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            DrawFoundation();
            DrawFormula1();
            DrawFormula2();
            DrawRoadLeft();
            DrawRoadRight();
            DrawWhiteLine();
            DrawRW();
            DrawLW();
            DrawText();
            gl.PopMatrix();

            gl.Flush();
        }

        public void ActivateAnimation()
        {
            AnimationActive = true;
            timer1.Start();
        }

        private void DefineTimers()
        {
            timer1 = new DispatcherTimer();
            timer2 = new DispatcherTimer();
            timer3 = new DispatcherTimer();
            timer4 = new DispatcherTimer();
            timer5 = new DispatcherTimer();
            timer6 = new DispatcherTimer();

            timer1.Interval = TimeSpan.FromMilliseconds(20);
            timer1.Tick += new EventHandler(ZoomScene);


            timer2.Interval = TimeSpan.FromMilliseconds(30);
            timer2.Tick += new EventHandler(TurnAroundCamera);


            timer3.Interval = TimeSpan.FromMilliseconds(20);
            timer3.Tick += new EventHandler(ZoomOut);


            timer4.Interval = TimeSpan.FromMilliseconds(1);
            timer4.Tick += new EventHandler(Formula1Wining);

            timer5.Interval = TimeSpan.FromMilliseconds(1);
            timer5.Tick += new EventHandler(Formula2Wining);

            timer6.Interval = TimeSpan.FromMilliseconds(1);
            timer6.Tick += new EventHandler(Finish);
        }

        private void ZoomScene(object sender, EventArgs e)
        {
           
            zoomInCounter++;
            m_sceneDistance -= 300.0f;

            if (zoomInCounter == 25)
            {
                timer1.Stop();
                timer2.Start();
               
            }
            timer3.Stop();
            timer4.Stop();
        }

        private void TurnAroundCamera(object sender, EventArgs e)
        {
            if (eyeZ > 5.0f)
                eyeZ -= 15.0f;
            else
            {
                timer2.Stop();
                timer3.Start();
            }
        }

        private void ZoomOut(object sender, EventArgs e)
        {
            zoomOutCounter++;

            m_sceneDistance += 200.0f;

            if (zoomOutCounter == 27)
            {
                timer3.Stop();
                timer4.Start();
            }
        }
        private void Formula1Wining(object sender, EventArgs e)
        {
           
            if(translatePoZ< -1800 && translatePoZBolid2< -2660)
            {
                translatePoZ += 100.0f;
                translatePoZBolid2 += 70.0f;
            }
            else
            {
                translatePoZBolid2 += 860.0f;
                timer4.Stop();
                timer5.Start();
            }
        }

        private void Formula2Wining(object sender, EventArgs e)
        {
            if (translatePoZBolid2 < 1040)
            {
                translatePoZ += 80.0f;
                translatePoZBolid2 += 120.0f;
            }
            else
            {
                timer5.Stop();

                timer6.Start();
            }
        }
        private void Finish(object sender, EventArgs e)
        {
            if (translatePoZ < 1040 )
                translatePoZ += 80.0f;
            else
            {
                timer6.Stop();
                AnimationActive = false;

            }
        }
        public void RefreshScene()
        {
            zoomInCounter = 0;
            zoomOutCounter = 0;
            eyeZ = 80.0f;
            eyeY = 30.0f;
            eyeX = 0.0f;
            centerZ = -15.0f;
            translatePoZ = -5000f;
            translatePoZBolid2 = -4900f;
            m_sceneDistance = 7000.0f;

        }
        private void DrawFormula1()
        {
           gl.Enable(OpenGL.GL_TEXTURE_2D); // omogucavanje upotrebe tekstura
           gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);
            gl.PushMatrix();
            gl.Translate(-730f, -2400f, translatePoZ);
            gl.Scale(skaliranjeBolida1, skaliranjeBolida1, skaliranjeBolida1);
             gl.Rotate(180, 0.0f, 1.0f, 0.0f);
            gl.Translate(transliranjeLevogBolida,1.1f, 0.0f);
            m_scene_firstCar.Draw();
            gl.PopMatrix();
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);
           gl.Disable(OpenGL.GL_TEXTURE_2D);
        }
        private void DrawFormula2()
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D); // omogucavanje upotrebe tekstura
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);
            gl.PushMatrix();
            gl.Translate(730f, -2250f, translatePoZBolid2);
            gl.Scale(skaliranjeBolida2, skaliranjeBolida2, skaliranjeBolida2);
             gl.Rotate(rotacijaDesnogBolida, 0.0f, 1.0f, 0.0f);

            m_scene_secondCar.Draw();
            gl.PopMatrix();
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        private void DrawFoundation()
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Sljunak]); //tekstura sljunka
            gl.MatrixMode(OpenGL.GL_TEXTURE);
            gl.Scale(3.0f, 2.0f, 2.0f);
            gl.LoadIdentity();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);


            gl.PushMatrix();
             gl.Translate(0f, 700f, 6500f);
             gl.Scale(20.0f, 20.0f, 15.0f);
            gl.Color(0.658824f, 0.658824f, 0.658824f);

             gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(0.0f, 1.0f, 0.0f);
            gl.TexCoord(0.0f, 10.0f);
            gl.Vertex(110f, -150f, -300);
            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(110f, -150f, -900);
            gl.TexCoord(10.0f, 0.0f);
            gl.Vertex(-110f, -150f, -900);
            gl.TexCoord(10.0f, 10.0f);
            gl.Vertex(-110f, -150f, -300);
            gl.End();
            gl.PopMatrix();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }
        private void DrawWhiteLine()
        {
            gl.PushMatrix();
            gl.Translate(0f, 703f, 6500f);
            gl.Scale(0.5f, 20.0f, 15.0f);
            gl.Color(0.9,0.9, 0.9);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(0.0f, 1.0f, 0.0f);
            gl.Vertex(110f, -150f, -300);
            gl.Vertex(110f, -150f, -900);
            gl.Vertex(-110f, -150f, -900);
            gl.Vertex(-110f, -150f, -300);
            gl.End();
            gl.PopMatrix();
        }
        private void DrawRoadLeft()
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Asfalt]); //tekstura asfalta
            gl.PushMatrix();
            gl.Translate(-600f, 702f, 6500f);
            gl.Scale(9.0f, 20.0f, 15.0f);
            gl.Color(0.8f, 0.9f, 0.8f, 1.0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(0.0f, 1.0f, 0.0f);
            gl.TexCoord(0.0f, 10.0f);
            gl.Vertex(110f, -150f, -300);
            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(110f, -150f, -900);
            gl.TexCoord(10.0f, 0.0f);
            gl.Vertex(-110f, -150f, -900);
            gl.TexCoord(10.0f, 10.0f);
            gl.Vertex(-110f, -150f, -300);
            gl.End();
            gl.PopMatrix();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }
        private void DrawRoadRight()
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Asfalt]); //tekstura asfalta
            gl.PushMatrix();
            gl.Translate(600f, 702f, 6500f);
            gl.Scale(9.0f, 20.0f, 15.0f);
            gl.Color(0.8f, 0.9f, 0.8f, 1.0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(0.0f, 1.0f, 0.0f);
            gl.TexCoord(0.0f, 10.0f);
            gl.Vertex(110f, -150f, -300);
            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(110f, -150f, -900);
            gl.TexCoord(10.0f, 0.0f);
            gl.Vertex(-110f, -150f, -900);
            gl.TexCoord(10.0f, 10.0f);
            gl.Vertex(-110f, -150f, -300);
            gl.End();
            gl.PopMatrix();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        private void DrawRW()
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Metal]);
            gl.PushMatrix();
            gl.Translate(1590f, -2180f, -2450f);
            gl.Scale(100.0f, 100.0f, 4300.0f);

            gl.Color(0.184314, 0.309804, 0.309804);
            Cube rightWall = new Cube();
            rightWall.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }
        private void DrawLW()
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Metal]);
            gl.PushMatrix();
            gl.Translate(-1590f, -2180f, -2450f);
            gl.Scale(100.0f, 100.0f, 4300.0f);

            gl.Color(0.184314, 0.309804, 0.309804);
            Cube leftWall = new Cube();
            leftWall.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }
        private void DrawText()
        {
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                gl.LoadIdentity();
                gl.Perspective(50f, (double)m_width / m_height, 1f, 20000f);
                gl.Ortho2D(-1.2f, -0.1f, -1.0f, -0.2f);
                gl.MatrixMode(OpenGL.GL_MODELVIEW);
                gl.LoadIdentity();




                gl.FrontFace(OpenGL.GL_CW);
                gl.Color(1.0f, 0.0f, 0.0f);

                gl.PushMatrix();
                gl.Translate(800f, -630f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "Predmet: Racunarska grafika");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -632f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "________________________");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -670f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "Sk.god: 2021/22. ");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -672f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "______________");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -710f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "Ime: Dajana");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -712f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "__________");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -750f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "Prezime: Zlokapa");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -752f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "______________");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -790f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "Sifra zad: 8.2");
                gl.PopMatrix();

                gl.PushMatrix();
                gl.Translate(800f, -792f, 5000f);
                gl.Scale(35.0f, 35.0f, 35.0f);
                gl.DrawText3D("Helvetica", 14f, 1f, 0.6f, "___________");
                gl.PopMatrix();

       

            gl.FrontFace(OpenGL.GL_CCW);


            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(50f, (double)m_width / m_height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

        }

        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0, 0, m_width, m_height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(50f, (double)width / height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix

        }

    
        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene_secondCar.Dispose();
                m_scene_firstCar.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }

   
}
