import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { JsonSampleDataService } from 'src/services/json-sample-data.service';
import { Mod } from '../../model/model';


@Component({
  selector: 'app-items-user-modal',
  templateUrl: './items-user-modal.component.html',
  styleUrls: ['./items-user-modal.component.css']
})
export class ItemsUserModalComponent implements OnInit {

  constructor(
    public activeModal: NgbActiveModal,private service: JsonSampleDataService, ) {
      console.log("test");
     }

  ngOnInit(): void {
  }

}
