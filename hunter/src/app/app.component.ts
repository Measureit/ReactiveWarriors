import { environment } from './../environments/environment';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HubConnectionState } from '@microsoft/signalr';
import { GameService } from './core/game.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title: string;
  readonly console: Array<string>;
  command: any = `
  {
    "CustomerName": "ProconTEL team"
  }`;

  form: FormGroup | undefined;

  constructor(
    private gameService: GameService,
    private formBuilder: FormBuilder) {
    this.title = 'Hunter';
    this.console = new Array<string>();
  }
  ngOnInit() {
    this.form = this.formBuilder.group({
      ip: ['', Validators.required]
    });
    this.form.markAllAsTouched();
  }

  async start() {

    this.gameService.onconnected(async id => {
      //await this.gameService.off('gameCreated');
      await this.gameService.on('roomCreated', (command) => {
        this.console.push(`Received notification: ${JSON.stringify(this.command)}.`);
      });
    });
    await this.gameService.start();
  }

  async createRoom(){
    var t = await this.gameService.send('createRoom', 'roomName')
    console.info(t);
  }

  async stop() {
    await this.gameService.stop();
  }

  get state(): HubConnectionState {
    return this.gameService && this.gameService.state;
  }

  get isConnected(): boolean {
    return this.state === HubConnectionState.Connected;
  }

  clearConsole() {
    this.console.splice(0, this.console.length);
  }

  // createOrder() {
  //   this.clearConsole();
  //   this.console.push(`Sending POST command: ${JSON.stringify(this.command)}.`);
  //   this.gameService
  //     .post('create_order', JSON.parse(this.command))
  //     .then(x => this.console.push('Command sent.'));
  // }

  // getOrder() {
  //   this.clearConsole();
  //   this.console.push(`Sending GET command: ${JSON.stringify(this.command)}.`);
  //   this.gameService
  //     .get('create_order_sync', JSON.parse(this.command))
  //     .then(x => this.console.push(`Received: ${JSON.stringify(x)}.`));
  // }
}