import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import {HttpClient,HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
user;
  constructor(public navCtrl: NavController,private _http:HttpClient) {

  }
  getUser(){
    this._http.get('user/getUser').subscribe((response)=>this.user=response);
  }

}
