import { Component, OnInit } from '@angular/core';
import { GraficosService } from '../graficos/graficos.service';
declare var google: any;
declare var $: any;
declare var Morris: any;
declare var c3: any;

@Component({
  selector: 'app-graficos',
  templateUrl: './graficos.component.html',
  styleUrls: [
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class GraficosComponent implements OnInit {
  public pageTitle: string = 'Principal';
  public status = [];
  public data1: any[];
  public qtdAPagar: number = 0;
  public vlrAPagar: number = 0;
  public anoCorrente: string = '';
  public anoAnterior: string = '';
  public mesCorrente: string = '';
  public mesAnterior: string = '';
  public qtddocumento: string = '0';
  public qtNfServicoAprovadas: string = '0';
  public qtNfProdutoAprovadas: string = '0';
  public vlrmes: string = '0';
  
  public loggedclienteId: string = ' ';
  public ultimosColetados = [];
  public valorPorMesProduto = [];
  public valorPorMesServico = [];
  public valorPorTipo = [];
  public valorTipoMes = [];
  // public blogs: Blog[] = [];
  // public logs: Log[] = [];

  constructor(private service: GraficosService) {
    this.getStatusArquivoColetado();
  }

  public ngOnInit() {
    // this.isAdm = this.authProfile.usuarioProfile.currentUsuario.fornecedor == null ? true : false;
  }

  public getStatusArquivoColetado(): void {
    // this.status = [];
    // this.ultimosColetados = [];
    // this.valorPorMes = [];

    this.service.getStatus()
      .subscribe((res) => {
        if (res.success) {
          this.status = res.result;
          this.anoCorrente = new Date().getFullYear().toString();
          this.anoAnterior = (new Date().getFullYear() - 1).toString();
          this.mesCorrente = this.getMonthCurrent();
          this.mesAnterior = this.getMonthPrevius();

          this.qtddocumento = this.status[0].qtddocumento;
          this.qtNfServicoAprovadas = this.status[0].qtNfServicoAprovadas;
          this.qtNfProdutoAprovadas = this.status[0].qtNfProdutoAprovadas;
          this.vlrmes = this.status[0].vlrmes;
        }
      });

    this.service.getUltimosArquivoColetado()
      .subscribe((res) => {
        if (res.success) {
          this.ultimosColetados = res.result;
        }
      });

    this.service.getValorPorMesProduto()
      .subscribe((res) => {
        if (res.success) {
          this.valorPorMesProduto = res.result;
          this.LoadCharts('chartValorMesProduto', this.valorPorMesProduto);
        }
      });
      this.service.getValorPorMesServico()
      .subscribe((res) => {
        if (res.success) {
          this.valorPorMesServico = res.result;
          this.LoadCharts('chartValorMesServico', this.valorPorMesServico);
        }
      });
    this.service.getValorPorTipo()
      .subscribe((res) => {
        if (res.success) {
          this.valorPorTipo = res.result;
          this.LoadChartPorTipo(this.valorPorTipo);
        }
      });

    this.service.getValorTipoMes()
      .subscribe((res) => {
        if (res.success) {
          this.valorTipoMes = res.result;
          this.LoadChartValorTipoMes(this.valorTipoMes);
        }
      });

  }

  public getMonthCurrent(): string {
    var _month = new Date().getMonth();

    if (_month == 0)
      return 'Janeiro';
    else if (_month == 1)
      return 'Fevereiro';
    else if (_month == 2)
      return 'Março';
    else if (_month == 3)
      return 'Abril';
    else if (_month == 4)
      return 'Maio';
    else if (_month == 5)
      return 'Junho';
    else if (_month == 6)
      return 'Julho';
    else if (_month == 7)
      return 'Agosto';
    else if (_month == 8)
      return 'Setembro';
    else if (_month == 9)
      return 'Outubro';
    else if (_month == 10)
      return 'Novembro';
    else
      return 'Dezembro';
  }

  public getMonthPrevius(): string {
    var date = new Date();
    date.setMonth(date.getMonth() - 1);
    var _month = date.getMonth();

    if (_month == 0)
      return 'Janeiro';
    else if (_month == 1)
      return 'Fevereiro';
    else if (_month == 2)
      return 'Março';
    else if (_month == 3)
      return 'Abril';
    else if (_month == 4)
      return 'Maio';
    else if (_month == 5)
      return 'Junho';
    else if (_month == 6)
      return 'Julho';
    else if (_month == 7)
      return 'Agosto';
    else if (_month == 8)
      return 'Setembro';
    else if (_month == 9)
      return 'Outubro';
    else if (_month == 10)
      return 'Novembro';
    else
      return 'Dezembro';
  }

  private LoadCharts(element, valorPorMes): void {


    var Dashboard1 = function () {
      this.$realData = []
    };


    //creates Donut chart
    Dashboard1.prototype.createBarChart = function (element, data, xkey, ykeys, labels, lineColors) {
      Morris.Bar({
        element: element,
        data: data,
        xkey: xkey,
        ykeys: ykeys,
        labels: labels,
        hideHover: 'auto',
        resize: true, //defaulted to true
        gridLineColor: '#eeeeee',
        barSizeRatio: 0.2,
        barColors: lineColors,
        postUnits: ' Reais'
      });
    },

      Dashboard1.prototype.init = function (valorPorMes) {
        var $barData = [];

        for (var _i = 0; _i < valorPorMes.length; _i++) {
          $barData.push({ y: valorPorMes[_i].Mes, a: valorPorMes[_i].Total });
          //lstmultasPorANo.addRows([[valorPorMes[_i].Mes, +valorPorMes[_i].Total]]);
        }

      this.createBarChart(element, $barData, 'y', ['a'], ['Valor'], ['#D1C265']);


      },
      $.Dashboard1 = new Dashboard1, $.Dashboard1.Constructor = Dashboard1

    $.Dashboard1.init(valorPorMes);
  }

  private LoadChartPorTipo(lst): void {
    

    var GoogleChart = function () {
    };

    GoogleChart.prototype.createPieChart = function (selector, data, colors, is3D, issliced) {
      var options = {
        fontName: 'Hind Madurai',
        legend: { position: 'right', textStyle: { fontSize: 10, bold: false } },
        fontSize: 13,
        animation: {
          duration: 1000,
          easing: 'out',
        },
        chartArea: {
          left: 10,
          width: '100%',
          height: '100%',
        },
        colors: colors
      };

      if (is3D) {
        options['is3D'] = true;
      }

      if (issliced) {
        options['is3D'] = true;
        //options['pieSliceText'] = 'label';
        options['slices'] = {
          2: { offset: 0.15 },
          5: { offset: 0.1 }
        };
      }

      var pie_chart = new google.visualization.PieChart(selector);
      pie_chart.draw(data, options);
      return pie_chart;
    },
      GoogleChart.prototype.createColumnStackChart = function (selector, data, axislabel, colors) {
        var options = {
          fontName: 'Hind Madurai',
          legend: 'none',
          fontSize: 13,
          chartArea: {
            left: '5%',
            width: '90%',
            height: 350
          },
          vAxis: {
            title: axislabel,
            titleTextStyle: {
              fontSize: 14,
              italic: false
            },
            gridlines: {
              color: '#D1C265',
              count: 10
            },
            minValue: 0
          },
          colors: colors
        };

        var stackedcolumn_chart = new google.visualization.ColumnChart(selector);
        stackedcolumn_chart.draw(data, options);
        return stackedcolumn_chart;
      },

      GoogleChart.prototype.init = function (lst) {
        var $this = this;

        var sliced_Data = new google.visualization.DataTable();
        sliced_Data.addColumn('string', 'Language');
        sliced_Data.addColumn('number', 'Speakers (in millions)');

        for (var _i = 0; _i < lst.length; _i++) {
          sliced_Data.addRows([[lst[_i].nome == 'Outros' ? 'Demais' : lst[_i].nome, +lst[_i].qtd]
          ]);
        }

      this.createPieChart($('#3d-exploded-chart')[0], sliced_Data, ['#C27470', '#FAD68C', '#BBC0C4', '#f5707a', '#E8DCC6'], true, true);

        $(window).on('resize', function () {
          this.createPieChart($('#3d-exploded-chart')[0], sliced_Data, ['#C27470', '#FAD68C', '#BBC0C4', '#f5707a', '#E8DCC6'], true, true);
        });
      },
      $.GoogleChart = new GoogleChart, $.GoogleChart.Constructor = GoogleChart

    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(function () { $.GoogleChart.init(lst); });

  };

  private LoadChartValorTipoMes(lst): void {

    var GoogleChart2 = function () {

    };

      //creates Column Stacked
      GoogleChart2.prototype.createColumnStackChart = function (selector, data, axislabel, colors) {
        var options = {
          fontName: 'Hind Madurai',
          height: 400,
          fontSize: 13,
          chartArea: {
            left: '5%',
            width: '90%',
            height: 350
          },
          isStacked: true,
          tooltip: {
            textStyle: {
              fontName: 'Hind Madurai',
              fontSize: 14
            }
          },
          vAxis: {
            title: axislabel,
            titleTextStyle: {
              fontSize: 14,
              italic: false
            },
            gridlines: {
              color: '#f5f5f5',
              count: 10
            },
            minValue: 0
          },
          legend: {
            position: 'top',
            alignment: 'center',
            textStyle: {
              fontSize: 13
            }
          },
          colors: colors
        };

        var google_chart_data = google.visualization.arrayToDataTable(data);
        var stackedcolumn_chart = new google.visualization.ColumnChart(selector);
        stackedcolumn_chart.draw(google_chart_data, options);
        return stackedcolumn_chart;
      },
      //init
        GoogleChart2.prototype.init = function (lst) {
        var $this = this;

        //creating columns tacked chart
        this.data1 = [
          ['Genre', 'Peças', 'Serviços', 'Ambos']
        ];
        
        if (lst.length >= 4) {
          for (var _i = 0; _i < lst.length; _i++) {
            this.data1.push([lst[_i].Mes, lst[_i].Valor, lst[_i + 1].Valor, lst[_i + 2].Valor]);
            _i = _i + 3;
          }
          
          $this.createColumnStackChart($('#column-stacked-chart')[0], this.data1, 'Valor', ['#7DC5CA', '#4bd396', '#f9c851', '#CCD7CA']);

          $(window).on('resize', function () {
            $this.createColumnStackChart($('#column-stacked-chart')[0], this.data1, 'Valor', ['#7DC5CA', '#4bd396', '#f9c851', '#CCD7CA']);
          });
        }
      },
      //init GoogleChart
        $.GoogleChart2 = new GoogleChart2, $.GoogleChart.Constructor = GoogleChart2

    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(function () { $.GoogleChart2.init(lst); });

  }
}
