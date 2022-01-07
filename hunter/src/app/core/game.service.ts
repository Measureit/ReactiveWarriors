import { Inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, IHttpConnectionOptions, IStreamResult } from '@microsoft/signalr';
import { EnvironmentConfig, ENV_CONFIG } from './environment-config.interface';

@Injectable({
  providedIn: 'root'
})
export class GameService {

    private readonly connection: HubConnection;
    private connectedCallbacks: Array<(connectionId?: string) => void>;
    constructor(@Inject(ENV_CONFIG) config: EnvironmentConfig) {
        this.connection = new HubConnectionBuilder()
            .withAutomaticReconnect()
            .withUrl(`${config.environment.baseUrl}/games`, {})
            .build();
        this.connectedCallbacks = [];
    }

    get state(): HubConnectionState {
        return this.connection.state;
    }

    start(): Promise<void> {
        return this.startInternal();
    }

    private async startInternal(): Promise<void> {
        await this.connection.start();
        this.connectedCallbacks.forEach((c) => c.apply(this, [this.connection.connectionId!]));
    }

    stop(): Promise<void> {
        return this.connection.stop();
    }

    on(commandId: string, newMethod: (...args: any[]) => void): void{//Promise<any> {
        this.connection.on(commandId, newMethod);
        //return this.connection.invoke("AddToGroupAsync", commandId)
    }

    off(commandId: string): Promise<any> {
        this.connection.off(commandId);
        return this.connection.invoke("RemoveFromGroupAsync", commandId)
    }

    send(methodName: string, arg: any): Promise<void> {
        return this.connection.send(methodName, arg);
    }



    onclose(callback: (error?: Error) => void): void {
        this.connection.onclose(callback);
    }

    onreconnecting(callback: (error?: Error) => void): void {
        this.connection.onreconnecting(callback);
    }

    onreconnected(callback: (connectionId?: string) => void): void {
        this.connection.onreconnected(callback);
    }

    onconnected(callback: (connectionId?: string) => void): void {
        if (callback) {
            this.connectedCallbacks.push(callback);
            this.connection.onreconnected(callback);
        }
    }

    stream<T = any>(methodName: string, ...args: any[]): IStreamResult<T> {
        return this.connection.stream(methodName, ...args);
    }
}