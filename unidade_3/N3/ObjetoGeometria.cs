/**
  Autor: Dalton Solano dos Reis
**/

using System.Collections.Generic;
using CG_Biblioteca;

namespace gcgcg
{
  internal abstract class ObjetoGeometria : Objeto
  {
    protected List<Ponto4D> pontosLista = new List<Ponto4D>();
    int pontoSelecionado = -1;

    public ObjetoGeometria(char rotulo, Objeto paiRef) : base(rotulo, paiRef) { }

    protected override void DesenharGeometria()
    {
      DesenharObjeto();
    }
    protected abstract void DesenharObjeto();
    public void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
      if (pontosLista.Count.Equals(1))
        base.BBox.Atribuir(pto);
      else
        base.BBox.Atualizar(pto);
      base.BBox.ProcessarCentro();
    }

    public void PontosRemoverUltimo()
    {
      pontosLista.RemoveAt(pontosLista.Count - 1);
    }

    public void PontosRemoverSelecionado()
    {
      if(pontoSelecionado != -1)
      {
        pontosLista.RemoveAt(pontoSelecionado);
        pontoSelecionado = -1;
      }
    }

    protected void PontosRemoverTodos()
    {
      pontosLista.Clear();
    }

    public Ponto4D PontosUltimo()
    {
      return pontosLista[pontosLista.Count - 1];
    }

    public Ponto4D PontosSelecionado()
    {
      if(pontoSelecionado != -1)
        return pontosLista[pontoSelecionado];
      return null;
    }

    public void PontosLimparSelecionado()
    {
      pontoSelecionado = -1;
    }

    public void PontosAlterar(Ponto4D pto, int posicao)
    {
      pontosLista[posicao] = pto;
    }

    public int selecionarPonto(int posX, int posY)
    {
      int pontoMaisProximo = -1;
      double menorDistancia = 0;
      double distancia;
      double distanciaX;
      double distanciaY;
      for (var i = 0; i < pontosLista.Count; i++)
      {
        distanciaX = pontosLista[i].X - posX;
        distanciaY = pontosLista[i].Y - posY;

        if(distanciaX < 0)
          distanciaX *= -1;

        if(distanciaY < 0)
          distanciaY *= -1;

        distancia = distanciaX + distanciaY;
        if (pontoMaisProximo == -1 || distancia < menorDistancia)
        {
          pontoMaisProximo = i;
          menorDistancia = distancia;
        }
      }
      pontoSelecionado = pontoMaisProximo;
      return pontoSelecionado;
    }

    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }
  }
}