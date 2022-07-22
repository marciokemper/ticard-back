import { Injectable } from '@angular/core';
declare var $: any;

@Injectable()
export class LoadingService {
  Show() {
    $('body').loadingModal({
      position: 'auto',
      text: '',
      color: '#fff',
      opacity: '0.3',
      backgroundColor: 'rgb(0,0,0)',
      animation: 'wave'
    });

  };

  Hide() {
    $('body').loadingModal('destroy');
  };

}
