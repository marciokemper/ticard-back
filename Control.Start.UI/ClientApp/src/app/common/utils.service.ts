import { Injectable } from '@angular/core';
declare var toastr: any;

@Injectable()
export class UtilsService {


  constructor() {
    toastr.options = {
      "closeButton": false,
      "debug": false,
      "newestOnTop": false,
      "progressBar": false,
      "positionClass": "toast-top-right",
      "preventDuplicates": false,
      "onclick": null,
      "showDuration": "300",
      "hideDuration": "1000",
      "timeOut": "1000",
      "extendedTimeOut": "1000",
      "showEasing": "swing",
      "hideEasing": "linear",
      "showMethod": "fadeIn",
      "hideMethod": "fadeOut"
    }
  }

  MessageSucessoShow(message:string,title:string="Mensagem") {
    toastr["success"](message, title)

  };

  MessageWarningShow(message: string, title: string = "Atenção") {
    toastr["warning"](message, title)
  };

}
