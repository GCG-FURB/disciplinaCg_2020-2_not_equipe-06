/**
  Autor: Sara Helena Régis Theiss
**/

using System;
using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {
        int qntPontos;
        double raio;

        public Circulo (char rotulo, Objeto paiRef, int qntPontos, double raio, Ponto4D centro) : base(rotulo, paiRef)
        {
            this.qntPontos = qntPontos;
            this.raio = raio;

            Ponto4D p = new Ponto4D();
            double ang = 360/qntPontos;
            for (int i = 0; i < qntPontos; i++ )
            {
                p = Matematica.GerarPtosCirculo(ang*i, raio);
                base.PontosAdicionar(new Ponto4D(p.X+centro.X, p.Y+centro.Y));

            }
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
    }
}