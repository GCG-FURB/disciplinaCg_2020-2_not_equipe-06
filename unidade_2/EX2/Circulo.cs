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

        public Circulo (string rotulo, Objeto paiRef, int qntPontos, double raio) : base(rotulo, paiRef)
        {
            this.qntPontos = qntPontos;
            this.raio = raio;

            Ponto4D p = new Ponto4D();
            double ang = 360/qntPontos;
            for (int i = 0; i < qntPontos; i++ )
            {
                p = Matematica.GerarPtosCirculo(ang*i, raio);
                base.PontosAdicionar(p);
                base.PontosAdicionar(new Ponto4D(p.X, p.Y));

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