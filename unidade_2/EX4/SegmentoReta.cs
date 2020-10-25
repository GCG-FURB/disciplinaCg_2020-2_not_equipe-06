/**
  Autor: João Victor Braun Quintino
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;

namespace gcgcg
{
  internal class SegmentoReta : ObjetoGeometria
  {
    public SegmentoReta(string rotulo, Objeto paiRef, Ponto4D a, Ponto4D b) : base(rotulo, paiRef)
    { 
      base.PontosAdicionar(a);
      base.PontosAdicionar(b);
    }

    protected override void DesenharObjeto()
    {
      GL.Begin(base.PrimitivaTipo);
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex2(pto.X, pto.Y);
      }
      GL.End();
    }
    //TODO: melhorar para exibir não só a lsita de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Circulo: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}