using System;
using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {
        int qntPontos;
        double raio;
        public Ponto4D centro;

        public Circulo (string rotulo, Objeto paiRef, int qntPontos, double raio, Ponto4D c) : base(rotulo, paiRef)
        {
            this.qntPontos = qntPontos;
            this.raio = raio;
            centro = c;
            Ponto4D p = new Ponto4D();
            double ang = 360/qntPontos;
            for (int i = 0; i < qntPontos; i++ )
            {
                p = Matematica.GerarPtosCirculo(ang*i, raio);
                base.PontosAdicionar(new Ponto4D(p.X, p.Y));

            }
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(base.PrimitivaTipo); 
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X+centro.X, pto.Y+centro.Y);
            }
            GL.End();
        }
    }
}