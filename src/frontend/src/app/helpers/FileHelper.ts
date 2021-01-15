import { Injectable } from '@angular/core';
import { saveAs } from 'file-saver';

@Injectable({
    providedIn: 'root'
})
export class FileHelper {

    downloadItem(item: any, fileName: string) {
        const blob = new Blob([item], { type: 'application/pdf' });
        saveAs(blob, fileName);
    }
}
