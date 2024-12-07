import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  standalone: false,
  
  templateUrl: './about.component.html',
  styleUrl: './about.component.css'
})
export class AboutComponent {
  title = 'Про нас';
  description = 'Ми є компанією, що займається розробкою веб-додатків. Наша мета - створювати якісні лабораторні роботи.';
}
