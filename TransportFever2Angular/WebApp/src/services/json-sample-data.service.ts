import { Injectable } from '@angular/core';
import { Mod, RootObject } from '../model/model';
import * as file  from '../data/testJson.json';
import { MenuTree } from 'src/model/menuModel';
import { ItemsUserModalComponent } from 'src/app/items-user-modal/items-user-modal.component';

@Injectable({
  providedIn: 'root'
})
export class JsonSampleDataService {
  model:RootObject=file;
  saveMods:Mod[]=[];

  constructor() {
    this.saveMods = JSON.parse(localStorage.getItem("saveMods")||'[]');
   }

  getData(page:number,move:number,search:string){
    let start=(page-1)*50
    if(search=='')
      return this.model.Mods.slice(start,move);
    else
      return this.model.Mods.filter(x=>x.Title.includes(search)).slice(start,move);
  }

  getMenu():MenuTree[]{
    let menu:MenuTree[]=[];
    this.model.CategoriesMap.forEach(x=>{
      let index=menu.findIndex(z=>z.ParentElement==x.ParentElement)
      if(index<0){
        let item:MenuTree={
          ParentElement:x.ParentElement,
          ChildElements:[]
        }
        menu.push(item)
      }
      else{
        menu[index].ChildElements.push(x.ChildElement);
      }
    })
    return menu;
  }
  addMod(item:Mod){
    if(this.saveMods.findIndex(x=>x.Id==item.Id)<0)
      this.saveMods.push(item);
  }

  removeMod(item:Mod){
    let index=this.saveMods.findIndex(x=>x.Id==item.Id);
    this.saveMods.splice(index,1);
  }

  checkIfAdd(item:Mod){
    return (this.saveMods.findIndex(x=>x.Id==item.Id)===-1);
  }
}
