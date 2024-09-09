using System;
using System.Collections.Generic;
using System.Globalization;


//Obs: Voce é livre para implementar na linguagem de sua preferência, desde que respeite as funcionalidades e saídas existentes, além de aplicar os conceitos solicitados.

namespace TransacaoFinanceira
{
    class Program
    {

        static void Main(string[] args)
        {
            var Transacoes = new List<Transacao>
            { 
                new() {CorrelationId= 1, DataHoraString="09/09/2023 14:15:00", ContaOrigem= 938485762, ContaDestino= 2147483649, Valor= 150},
                new() {CorrelationId= 2, DataHoraString="09/09/2023 14:15:05", ContaOrigem= 2147483649, ContaDestino= 210385733, Valor= 149},
                new() {CorrelationId= 3, DataHoraString="09/09/2023 14:15:29", ContaOrigem= 347586970, ContaDestino= 238596054, Valor= 1100},
                new() {CorrelationId= 4, DataHoraString="09/09/2023 14:17:00", ContaOrigem= 675869708, ContaDestino= 210385733, Valor= 5300},
                new() {CorrelationId= 5, DataHoraString="09/09/2023 14:18:00", ContaOrigem= 238596054, ContaDestino= 674038564, Valor= 1489},
                new() {CorrelationId= 6, DataHoraString="09/09/2023 14:18:20", ContaOrigem= 573659065, ContaDestino= 563856300, Valor= 49},
                new() {CorrelationId= 7, DataHoraString="09/09/2023 14:19:00", ContaOrigem= 938485762, ContaDestino= 2147483649, Valor= 44},
                new() {CorrelationId= 8, DataHoraString="09/09/2023 14:19:01", ContaOrigem= 573659065, ContaDestino= 675869708, Valor= 150},

            };
            ExecutarTransacaoFinanceira executor = new ExecutarTransacaoFinanceira();
            foreach(var item in Transacoes)
            {
                executor.Transferir(item.CorrelationId, item.ContaOrigem, item.ContaDestino, item.Valor);
            };

        }
    }

    class ExecutarTransacaoFinanceira: AcessoDados
    {
        public void Transferir(int correlationId, long contaOrigem, long contaDestino, decimal valor)
        {
            decimal saldoOrigem = ObterSaldo(contaOrigem);
        
            if (saldoOrigem < valor)
            {
                Console.WriteLine("Transacao numero {0} foi cancelada por falta de saldo", correlationId);
            }
            else
            {
                decimal saldoDestino = ObterSaldo(contaDestino);
                
                AtualizarSaldo(contaOrigem, saldoOrigem - valor);
                AtualizarSaldo(contaDestino, saldoDestino + valor);
                
                Console.WriteLine("Transacao numero {0} foi efetivada com sucesso! Novos Saldos: Conta Origem:{1} | Conta Destino: {2}", 
                    correlationId, saldoOrigem - valor, saldoDestino + valor);
            }
        }
    }

    class Transacao
    {   
        public int CorrelationId { get; set; }
        public string DataHoraString { get; set; }
        public long ContaOrigem { get; set; }
        public long ContaDestino { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataHora
        {
            get
            {
                return DateTime.ParseExact(DataHoraString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }
    }

    class AcessoDados
    {
        private Dictionary<long, decimal> Saldos { get; set; }
        public AcessoDados()
        {            
            Saldos = new Dictionary<long, decimal>();
            this.Saldos.Add(938485762, 180);
            this.Saldos.Add(347586970, 1200);
            this.Saldos.Add(2147483649, 0);
            this.Saldos.Add(675869708, 4900);
            this.Saldos.Add(238596054, 478);
            this.Saldos.Add(573659065, 787);
            this.Saldos.Add(210385733, 10);
            this.Saldos.Add(674038564, 400);
            this.Saldos.Add(563856300, 1200);
           
        }
        public decimal ObterSaldo(long id)
        {
            if (Saldos.TryGetValue(id, out var saldo))
            {
                return saldo;
            }
            else
            {
                throw new KeyNotFoundException($"Conta {id} não encontrada.");
            }
        }

        public bool AtualizarSaldo(long conta, decimal saldo)
        {
            if (!Saldos.ContainsKey(conta))
            {
                throw new InvalidOperationException($"A conta {conta} não existe.");
            }

            Saldos[conta] = saldo;
            return true;
        }


    }
}
