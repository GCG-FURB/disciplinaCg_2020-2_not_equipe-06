/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;

namespace gcgcg
{
  class Mundo : GameWindow
  {
    private static Mundo instanciaMundo = null;

    private Mundo(int width, int height) : base(width, height) { }

    public static Mundo GetInstance(int width, int height)
    {
      if (instanciaMundo == null)
        instanciaMundo = new Mundo(width, height);
      return instanciaMundo;
    }

    private CameraOrtho camera = new CameraOrtho();
    protected List<Objeto> objetosLista = new List<Objeto>();
    private ObjetoGeometria objetoSelecionado = null;
    private bool bBoxDesenhar = false;
    int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
    private bool mouseMoverPto = false;
    // private Retangulo obj_Retangulo;
    private Circulo obj_Circulo;
    private Circulo obj_Circulo2;
    private Circulo obj_Circulo3;

#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = -300; camera.xmax = 300; camera.ymin = -300; camera.ymax = 300;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");         
      
      obj_Circulo = new Circulo("A", null, 72, 100, new Ponto4D(0,0));
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 0; obj_Circulo.ObjetoCor.CorB = 0;
      obj_Circulo.PrimitivaTamanho = 5;
      obj_Circulo.PrimitivaTipo = PrimitiveType.Points;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
      // obj_Circulo.DesenharObjeto();

#if CG_Privado
      obj_SegReta = new Privado_SegReta("B", null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      obj_Circulo = new Privado_Circulo("C", null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
      GL.ClearColor(1f,1f,1f,1f); // cor de fundo da tela em branco
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
#if CG_Gizmo      
      Sru3D();
#endif
      for (var i = 0; i < objetosLista.Count; i++)
        objetosLista[i].Desenhar();
      if (bBoxDesenhar && (objetoSelecionado != null))
        objetoSelecionado.BBox.Desenhar();
      this.SwapBuffers();
    }

    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {
      int limiteMin = -100;
      int limiteMax = 100;

      /*
      // tecla Pan (deslocar para esquerda): E;
      // tecla Pan (deslocar para direita): D;
      // tecla Pan (deslocar para cima): C;
      // tecla Pan (deslocar para baixo): B;
      // tecla Zoom in (aproximar): I;
      // tecla Zoom out (afastar): O.
      */
      if (e.Key == Key.H)
        Utilitario.AjudaTeclado();
      else if (e.Key == Key.Escape)
        Exit();
      else if (e.Key == Key.J)
      {
        Console.WriteLine("xmin " + camera.xmin);
        Console.WriteLine("xmax " + camera.xmax);
        Console.WriteLine("ymin " + camera.ymin);
        Console.WriteLine("ymax " + camera.ymax + "\n");
      }
      else if (e.Key == Key.E)// tecla Pan (deslocar para esquerda)
      {
        if(camera.xmin + 10 <= limiteMin)
        {
          camera.xmin += 10; camera.xmax += 10;
        }
      }
      else if (e.Key == Key.D)// tecla Pan (deslocar para direita)
      {
        if(camera.xmax - 10 >= limiteMax)
        {
          camera.xmin -= 10; camera.xmax -= 10;
        }
      }
      else if (e.Key == Key.C)// tecla Pan (deslocar para cima)
      {
        if(camera.ymax - 10 >= limiteMax)
        {
          camera.ymin -= 10; camera.ymax -= 10;
        }
      }
      else if (e.Key == Key.B)// tecla Pan (deslocar para baixo)
      {
        if(camera.ymin + 10 <= limiteMin)
        {
          camera.ymin += 10; camera.ymax += 10;
        }
      }
      else if (e.Key == Key.I)// tecla Zoom in (aproximar)
      {
        if(camera.xmin + 10 <= limiteMin && camera.xmax - 10 >= limiteMax
          && camera.ymin + 10 <= limiteMin && camera.ymax - 10 >= limiteMax)
        {
          camera.xmin += 10; camera.xmax -= 10; camera.ymin += 10; camera.ymax -= 10;
        }
      }
      else if (e.Key == Key.O)// tecla Zoom out (afastar)
      {
        camera.xmin -= 10; camera.xmax += 10; camera.ymin -= 10; camera.ymax += 10;
      }
      else
        Console.WriteLine(" __ Tecla não implementada.");
    }

    //TODO: não está considerando o NDC
    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
      if (mouseMoverPto && (objetoSelecionado != null))
      {
        objetoSelecionado.PontosUltimo().X = mouseX;
        objetoSelecionado.PontosUltimo().Y = mouseY;
      }
    }

#if CG_Gizmo
    private void Sru3D()
    {
      // desenhando os eixos positivos x e y, ambos com comprimento 200
      GL.LineWidth(1);
      GL.Begin(PrimitiveType.Lines);
      // GL.Color3(1.0f,0.0f,0.0f);
      GL.Color3(Convert.ToByte(255),Convert.ToByte(0),Convert.ToByte(0));
      GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
      // GL.Color3(0.0f,1.0f,0.0f);
      GL.Color3(Convert.ToByte(0),Convert.ToByte(255),Convert.ToByte(0));
      GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
      GL.End();
    }
#endif    
  }
  class Program
  {
    static void Main(string[] args)
    {
      Mundo window = Mundo.GetInstance(600, 600);
      window.Title = "CG_N2";
      window.Run(1.0 / 60.0);
    }
  }
}
