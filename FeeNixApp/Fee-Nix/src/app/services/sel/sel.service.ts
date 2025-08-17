import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class SelService {
    jwtoken: string;

    constructor(private http: HttpClient) {
        const storedUser = localStorage.getItem("sel-bill-user");
        let token = storedUser ? JSON.parse(storedUser) : {};
        this.jwtoken = token.jwtToken;
    }

    private createHeaders(token: string) {
        return new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        });
    }

    getData(url: string) {
        const headers = this.createHeaders(this.jwtoken);
        return this.http.get<any>(url, { headers });
    }

    getUploadData(url: string) {
        const headers = this.createHeaders(this.jwtoken);
        return this.http.get(url, { headers, responseType: 'arraybuffer' });
    }

    sendData(url: string, data: any) {
        const headers = this.createHeaders(this.jwtoken);
        return this.http.post<any>(url, data, { headers });
    }

    patchData(url: string, data: any) {
        const headers = this.createHeaders(this.jwtoken);
        return this.http.patch<any>(url, data, { headers });
    }

    putData(url: string, data: any) {
        const headers = this.createHeaders(this.jwtoken);
        return this.http.put<any>(url, data, { headers });
    }

    deleteData(url: string) {
        const headers = this.createHeaders(this.jwtoken);
        return this.http.delete<any>(url, { headers });
    }
}