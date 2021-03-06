import { Component, OnInit , Input} from '@angular/core';
import {EncargadoEvento,EncargadoEventoServices} from '../../servicios/encargado.service';
import {Domicilio} from '../../servicios/socio.service';
import {FormGroup} from '@angular/forms';
import {DatePipe} from '@angular/common' ;
import { ToastrService, ToastrConfig } from 'ngx-toastr';
//import { TipoDocumentoComponent } from '../tipo-documento/tipo-documento.component';
import { TipoDocumento } from '../../servicios/tipo-documento.service';

@Component({
  selector: 'az-encargados',
  templateUrl: './encargados.component.html',
  styleUrls: ['./encargados.component.scss']
})
export class EncargadosComponent implements OnInit {

  public _item = new EncargadoEvento(0,"","");
  @Input() items = new Array<EncargadoEvento>();
  public showDetail:boolean = false;

  constructor(private _itemsService:EncargadoEventoServices,private mensajeServ: ToastrService)
  {
  }

  ngOnInit()
  {
    this.getItems();
  }

  getItems()
  {
    this._itemsService.query({}).subscribe(items => {
      this.items = items;
    });
  }

  onSubmit(myForm: FormGroup)
  {
    let newEspacio = Object.assign({}, this._item);
    this._item = new EncargadoEvento(0,"","");
    this.saveItem(newEspacio)
    myForm.reset();
  }

  verItem(item:EncargadoEvento)
  {
    this._itemsService.get(item.id).subscribe(resp => {
      this._item = resp;
      this.showDetail = true;
    });
  }
  nuevoItem()
  {
    this._item =  new EncargadoEvento(0,"","","");
    this._item.domicilio = new Domicilio();
    this.showDetail = true;
  }
  limpiarForm()
  {
    this._item =  new EncargadoEvento(0,"","","");
    this.showDetail = false;
  }


  saveItem(item:EncargadoEvento):any
  {
    if(item.id == 0)
    {
      this._itemsService.save(item).subscribe(resp => {
        item = resp;
        this.items.push(item);
        this.showDetail = false;
        this.mensajeServ.success('Se han guardado los cambios!', 'Aviso!');
      });
    }
    else
    {
      this._itemsService.update(item).subscribe(resp => {
        let items = this.items;
        for (var i = 0; i < items.length; i++)
        {
          if(items[i].id == resp.id)
          { items[i] = resp;
            this.mensajeServ.success('Se han guardado los cambios!', 'Aviso!');
          }
        }
        this.showDetail = false;
      });
    }
  }
}
