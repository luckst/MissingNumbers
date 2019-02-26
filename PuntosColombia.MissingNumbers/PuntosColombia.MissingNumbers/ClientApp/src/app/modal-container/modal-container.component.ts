import { Component, Input } from '@angular/core';
import { Search } from '../models/search';

@Component({
  selector: 'app-modal-container',
  templateUrl: './modal-container.component.html',
  styleUrls: ['./modal-container.component.css']
})
/** modal-container component*/
export class ModalContainerComponent {
  /** modal-container ctor */
  @Input() public search;
  constructor() {

  }
}
