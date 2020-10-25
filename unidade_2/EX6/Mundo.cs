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
    private Spline obj_Spline;
    private List<PrimitiveType> tipos = new List<PrimitiveType>();
    private int indexTipo = 0;
    private SegmentoReta obj_SegReta1;
    private SegmentoReta obj_SegReta2;
    private SegmentoReta obj_SegReta3;
    
    
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = -400; camera.xmax = 400; camera.ymin = -400; camera.ymax = 400;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");         
      
      tipos.Add(PrimitiveType.Points);
      tipos.Add(PrimitiveType.Lines);
      tipos.Add(PrimitiveType.LineLoop);
      tipos.Add(PrimitiveType.LineStrip);
      tipos.Add(PrimitiveType.Triangles);
      tipos.Add(PrimitiveType.TriangleStrip);
      tipos.Add(PrimitiveType.TriangleFan);
      tipos.Add(PrimitiveType.Quads);
      tipos.Add(PrimitiveType.QuadStrip);
      tipos.Add(PrimitiveType.Polygon);

      obj_SegReta1 = new SegmentoReta("4",null,new Ponto4D(100,-100),new Ponto4D(100,100));
      obj_SegReta1.ObjetoCor.CorR = 0; obj_SegReta1.ObjetoCor.CorG = 255; obj_SegReta1.ObjetoCor.CorB = 242;
      obj_SegReta1.PrimitivaTamanho = 2;
      objetosLista.Add(obj_SegReta1);

      obj_SegReta2 = new SegmentoReta("4",null,new Ponto4D(100,100),new Ponto4D(-100,100));
      obj_SegReta2.ObjetoCor.CorR = 0; obj_SegReta2.ObjetoCor.CorG = 255; obj_SegReta2.ObjetoCor.CorB = 242;
      obj_SegReta2.PrimitivaTamanho = 2;
      objetosLista.Add(obj_SegReta2);

      obj_SegReta3 = new SegmentoReta("4",null,new Ponto4D(-100,100),new Ponto4D(-100,-100));
      obj_SegReta3.ObjetoCor.CorR = 0; obj_SegReta3.ObjetoCor.CorG = 255; obj_SegReta3.ObjetoCor.CorB = 242;
      obj_SegReta3.PrimitivaTamanho = 2;
      objetosLista.Add(obj_SegReta3);

      obj_Spline = new Spline("S", null, 10, new Ponto4D(100, -100), new Ponto4D(100, 100), new Ponto4D(-100, 100), new Ponto4D(-100, -100));
      obj_Spline.ObjetoCor.CorR = 255; obj_Spline.ObjetoCor.CorG = 255; obj_Spline.ObjetoCor.CorB = 0;
      obj_Spline.PrimitivaTamanho = 2;
      obj_Spline.PrimitivaTipo = PrimitiveType.LineStrip;
      
      obj_Spline.bMostrarPontos = true;
      obj_Spline.PontosCor.CorR = 0; obj_Spline.PontosCor.CorG = 0; obj_Spline.PontosCor.CorB = 0;
      obj_Spline.PontoSelecionadoCor.CorR = 255; obj_Spline.PontoSelecionadoCor.CorG = 0; obj_Spline.PontoSelecionadoCor.CorB = 0;
      obj_Spline.PontosTamanho = 6;

      objetosLista.Add(obj_Spline);
      objetoSelecionado = obj_Spline;

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

    private void AlterarSegsReta(int ponto, double x, double y)
    {
      switch(ponto)
        {
          case 1:
            obj_SegReta1.SomarPonto(0, x, y);
            break;
          case 2:
            obj_SegReta1.SomarPonto(1, x, y);
            obj_SegReta2.SomarPonto(0, x, y);
            break;
          case 3:
            obj_SegReta2.SomarPonto(1, x, y);
            obj_SegReta3.SomarPonto(0, x, y);
            break;
          case 4:
            obj_SegReta3.SomarPonto(1, x, y);
            break;
        }
    }

    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {
      int limiteMin = -100;
      int limiteMax = 100;
      int txDesloca = 10; // quantidade de pontos deslocados em cada interação que envolve movimentação
      Spline obj = (Spline) objetoSelecionado;

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
      else if (e.Key == Key.E)// deslocar para esquerda
      {
        obj.SomarPontoControle(-txDesloca, 0);
        AlterarSegsReta(obj.PontoSelecionado, -txDesloca, 0);
      }
      else if (e.Key == Key.D)// deslocar para direita
      {
        obj.SomarPontoControle(10, 0);
        AlterarSegsReta(obj.PontoSelecionado, txDesloca, 0);
      }
      else if (e.Key == Key.C)// deslocar para cima
      {
        obj.SomarPontoControle(0, 10);
        AlterarSegsReta(obj.PontoSelecionado, 0, txDesloca);
      }
      else if (e.Key == Key.B)// deslocar para baixo
      {
        obj.SomarPontoControle(0, -10);
        AlterarSegsReta(obj.PontoSelecionado, 0, -txDesloca);
      }
      else if (e.Key == Key.I)// tecla Zoom in (aproximar)
      {
        if(camera.xmin + txDesloca <= limiteMin && camera.xmax - txDesloca >= limiteMax
          && camera.ymin + txDesloca <= limiteMin && camera.ymax - txDesloca >= limiteMax)
        {
          camera.xmin += txDesloca; camera.xmax -= txDesloca; camera.ymin += txDesloca; camera.ymax -= txDesloca;
        }
      }
      else if (e.Key == Key.O)// tecla Zoom out (afastar)
      {
        camera.xmin -= txDesloca; camera.xmax += txDesloca; camera.ymin -= txDesloca; camera.ymax += txDesloca;
      }
      else if(e.Key == Key.Space){
        indexTipo++;
        if(indexTipo>9)
          indexTipo = 0;
        objetoSelecionado.PrimitivaTipo = tipos[indexTipo];
      }
      else if (e.Key == Key.Y)
        bBoxDesenhar = !bBoxDesenhar;
      else if (e.Key == Key.Number1 || e.Key == Key.Keypad1)
      {
        obj.PontoSelecionado = 1;
      }
      else if (e.Key == Key.Number2 || e.Key == Key.Keypad2)
      {
        obj.PontoSelecionado = 2;
      }
      else if (e.Key == Key.Number3 || e.Key == Key.Keypad3)
      {
        obj.PontoSelecionado = 3;
      }
      else if (e.Key == Key.Number4 || e.Key == Key.Keypad4)
      {
        obj.PontoSelecionado = 4;
      }
      else if (e.Key == Key.R)
      {
        obj.RestaurarValoresIniciais();
        obj_SegReta1.RestaurarValoresIniciais();
        obj_SegReta2.RestaurarValoresIniciais();
        obj_SegReta3.RestaurarValoresIniciais();
      }
      else if (e.Key == Key.Minus || e.Key == Key.KeypadMinus)
      {
        if(obj.qtdPontos > 1)
          obj.qtdPontos -= 1;
      }
      else if (e.Key == Key.Plus || e.Key == Key.KeypadAdd)
      {
        obj.qtdPontos += 1;
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
