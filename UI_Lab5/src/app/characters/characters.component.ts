import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { CommonModule } from '@angular/common';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { Router } from '@angular/router';

export interface Character {
  characterId: number;
  name: string;
  description: string;
  strength: number;
  speed: number;

}

export interface Player {
  playerId: number;
  nickname: string;
  email: string;
  sex: string;
  registrationDate: string;
  registrationTime: string;
  score: number;
}

@Component({
  selector: 'app-characters',
  standalone: true,
  
  templateUrl: './characters.component.html',
  styleUrl: './characters.component.css',
  imports: [ReactiveFormsModule, CommonModule]
})
export class CharactersComponent {
  characterForm: FormGroup;

  characters: Character[] = [];

  character: Character[] = [];

  isEditCharacter = false;
  isAddCharacter = false;

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private router: Router
  ) {
    this.loadCharacters();
    this.characterForm = this.fb.group(
      {
        name: ['', [Validators.required]] ,
        description: ['', [Validators.required]],
        strength: ['', [Validators.required]],
        speed: ['', [Validators.required]],
      }
    );
  }

  isInvalid(controlName: string): boolean {
    const control = this.characterForm.get(controlName);
    let res = !!(control?.invalid && (control?.touched || control?.dirty));
    return res;
  }

  getMaxCharacterId(characters: Character[]): number {
  if (!characters || characters.length === 0) {
    return -1; // Повертаємо null, якщо масив порожній
  }

  return Math.max(...characters.map(character => character.characterId));
}

  ngOnInit(): void { }

  loadCharacters() {
    this.apiService.getData('Players').subscribe({
      next: (response) => {
        let email = localStorage.getItem('email');
        if (email) {
          let foundPlayer = this.findPlayerByEmail(response, email);
          if (foundPlayer) {
            let playerId = foundPlayer.playerId;
            this.apiService.getData(`PlayersCharacters/filter/byPlayer/${playerId}`).subscribe({
              next: (response) => {
                let characterIdsArray = this.getCharacterIds(response);
                this.apiService.getData('Characters').subscribe({
                  next: (response) => {
                    this.characters = this.getCharactersByIds(response, characterIdsArray);
                  },
                  error: (error) => {
                    console.error(error);
                  }
                })
              },
              error: (error) => {
                console.error(error);
              }
            })
          }
        }
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  findPlayerByEmail(players: Player[], email: string): Player | undefined {
    return players.find(player => player.email === email);
  }

  getCharacterIds(data: { playerId: number, characterId: number }[]): number[] {
    return data.map(item => item.characterId);
  }

  getCharactersByIds(characters: Character[], ids: number[]): Character[] {
    return characters.filter(character => ids.includes(character.characterId));
  }

  addCharacter() {
    this.isAddCharacter = true;
    this.isEditCharacter = false;
  }

  editCharacter(toEditCharacter: Character) {
    this.isEditCharacter = true;
    this.isAddCharacter = false;
    if (this.character.length > 0) {
      this.character[0] = { ...toEditCharacter };
    } else {
      this.character.push({
        characterId: toEditCharacter.characterId,
        name: toEditCharacter.name,
        description: toEditCharacter.description,
        strength: toEditCharacter.strength,
        speed: toEditCharacter.speed
      });
    }
  }

  deleteCharacter(toDelCharacter: Character) {
    this.apiService.deleteData('Characters/' + toDelCharacter.characterId).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }


  onSubmit() {
    console.log("sad00");
    console.log(this.characterForm.valid);
    if (this.characterForm.valid) {
      

      if (this.isEditCharacter) {

        var data = {
            characterId: this.character[0].characterId,
            name: this.characterForm.value.name,
            description: this.characterForm.value.description,
            strength: this.characterForm.value.strength,
            speed: this.characterForm.value.speed
          };

        this.isEditCharacter = false;
        this.apiService.putCharacterData('Characters', data).subscribe({
          next: (response) => {
            console.log(response);
          },
          error: (error) => {
            console.error(error);
          },
        });
      }

      if (this.isAddCharacter) {

        this.apiService.getData('Characters').subscribe({
          next: (response) => {
            let id = this.getMaxCharacterId(response) + 1;
            if (id == 0) return;
            var data = {
              characterId: id,
              name: this.characterForm.value.name,
              description: this.characterForm.value.description,
              strength: this.characterForm.value.strength,
              speed: this.characterForm.value.speed
            };

            this.isAddCharacter = false;
            this.apiService.postData('Characters', data).subscribe({
              next: (response) => {
                console.log(response);
              },
              error: (error) => {
                console.error(error);
              },
            });

            this.apiService.getData('Players').subscribe({
              next: (response) => {
                let email = localStorage.getItem('email');
                if (email) {
                  let foundPlayer = this.findPlayerByEmail(response, email);
                  if (foundPlayer) {
                    var dataPlayerCharacter = {
                      playerId: foundPlayer.playerId,
                      characterId: data.characterId
                    }

                    this.apiService.postData('PlayersCharacters', dataPlayerCharacter).subscribe({
                      next: (response) => {
                        console.log(response);
                      },
                      error: (error) => {
                        console.error(error);
                      }
                    })
                  }
                }
              },
              error: (error) => {
                console.error(error);
              }
              
            })
          },
          error: (error) => {

          }
        })
        
        
      }

    }
  }
}
