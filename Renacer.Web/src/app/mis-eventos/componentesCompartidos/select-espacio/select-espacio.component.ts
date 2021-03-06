import { Component, OnInit,Input,OnChanges } from '@angular/core';
import {EspacioServices,EspacioComun} from '../../../servicios/espacio.service';

@Component({
  selector: 'select-espacio',
  template:`<div class="form-group">
  <label>Espacio Comun
  </label>
  <select class="form-control" id="select_item" (change)="actualizarItem(select_item)"
  required #select_item="ngModel" name="select_item" [(ngModel)]="itemSelected">
  <option *ngFor="let item of items;trackBy:item?.id" [ngValue]="item">{{item.nombre}}
  </option>
  </select>
  <div [hidden]="select_item.valid || select_item.pristine" class="alert alert-danger">
  Debe seleccionar el Espacio
  </div>
  </div>
  `
})
export class SelectEspacioComponent implements OnInit {

  @Input() item:EspacioComun;
  public itemSelected:EspacioComun;
  public items = new Array<EspacioComun>();
  constructor(private _dbServices:EspacioServices) {
  }

  ngOnInit() {
    this.getItems();
    console.log("item:" + this.item);
  }

  ngOnChanges(){
    this.setItemSelected();
  }

  setItemSelected(){
    if(this.items.length != 0 && this.item != null){
      this.itemSelected = this.item;

      for(var i = 0; i < this.items.length;i++){
        if(this.item.id == this.items[i].id){
          this.item.nombre = this.items[i].nombre;
          this.items[i] = this.item;
          this.itemSelected = this.items[i];
        }
      }
    }
  }

  actualizarItem(_item:EspacioComun){
    if(this.item){
    this.item.id = this.itemSelected.id;
    this.item.nombre = this.itemSelected.nombre;
    }else{
      this.item = new EspacioComun(this.itemSelected.id,this.itemSelected.nombre)
    }
  }

  getItems(){
    this._dbServices.query({}).subscribe(items => {
      this.items = [];
      for(var i = 0; i < items.length;i++){
        var itemAux = new EspacioComun(0);
        itemAux.id = items[i].id;
        itemAux.nombre = items[i].nombre;
        this.items.push(itemAux);
      }
      this.setItemSelected();
    });
  }

}
