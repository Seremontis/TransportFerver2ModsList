import { Component,HostListener,Input, OnInit } from '@angular/core';
import {JsonSampleDataService} from '../../services/json-sample-data.service';
import { Mod } from '../../model/model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ItemsUserModalComponent } from '../items-user-modal/items-user-modal.component';
import { MenuTree } from 'src/model/menuModel';


@Component({
  selector: 'app-list-mods',
  templateUrl: './list-mods.component.html',
  styleUrls: ['./list-mods.component.css'],
})
export class ListModsComponent implements OnInit {
  mods: Mod[] = [];
  searchValue:string ='';
  page:number=1;
  addItemsCount:number=20;


  constructor(private service: JsonSampleDataService,private modalService: NgbModal) {
  }

  ngOnInit(): void {
    this.mods=this.service.getData(1,this.addItemsCount,this.searchValue);
    //this.resize(window);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    //this.resize(event.target)
  }

  searchMods(val:any){
    this.searchValue=val.form.controls.message.value.trim();
    this.mods=this.service.getData(1,this.addItemsCount,this.searchValue);
  }

  getMenu():MenuTree[]{
    return this.service.getMenu();
  }

  addMod(item:Mod){
    this.service.addMod(item);
  }

  removeMod(item:Mod){
    this.service.removeMod(item);
  }

  checkIfAdd(item:Mod):Boolean{
    return this.service.checkIfAdd(item);
  }

  getCurrentModsCount():number{
    return this.service.saveMods.length;
  }

  openModal() {
    const modalRef = this.modalService.open(ItemsUserModalComponent, {
      size: 'xl',
      centered: true,
      windowClass: 'dark-modal'
    });
  }  

  @HostListener("window:beforeunload", ["$event"]) unloadHandler(event: Event) {
    localStorage.setItem("saveMods",'');
    localStorage.setItem("saveMods", JSON.stringify(this.service.saveMods));
}
  /* private resize(event:any){
    /*if (event.innerWidth > 1400) {
      document.querySelectorAll('.card-group>div').forEach((el) => {
        this.removeClass(el);
        el.classList.add('col-2');
      });
    } else
    if (event.innerWidth > 1300) {
      document.querySelectorAll('.card-group>div').forEach((el) => {
        this.removeClass(el);
        el.classList.add('col-3');
      });
    } else {
      document.querySelectorAll('.card-group>div').forEach((el) => {
        this.removeClass(el);
        el.classList.add('col-4');
      });
    }
  }

  private removeClass(el: any){
    el.classList.remove('col-3');
    el.classList.remove('col-4');
    //el.classList.remove('col-6');
    //el.classList.remove('col-2');
  }*/
}
