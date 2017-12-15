import { Component, Inject, OnInit } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
    public forecasts: Service[];
    private http: Http;
    private baseUrl: string;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
            this.forecasts = result.json() as Service[];
        }, error => console.error(error));
        this.http = http;
        this.baseUrl = baseUrl;
    }

    data(dataEntity: Service) {
        console.log(dataEntity.url);
        var body = dataEntity;
        
        var headers = new Headers();
        headers.append('Content-Type', 'application/json');
        
        console.log(body);
        const req = this.http
            .post(this.baseUrl + 'api/SampleData/WeatherForecasts',
            body, {
                headers: headers
            })
            .subscribe(data => {
                console.log(data);
                var text = data.text("legacy");
                var blob = new Blob([text], { type: 'text/plain' });
                var url = window.URL.createObjectURL(blob);
                window.open(url);
            }, error => {
                console.log(JSON.stringify(error.json()));
            });
    }
}

interface Service {
    url: string;
    methodValue: string;
    dataTypes: string;
    bodyLabels: string;
    method: string;
}
