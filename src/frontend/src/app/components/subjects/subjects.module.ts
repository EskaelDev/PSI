import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SharedComponentsModule } from '../shared/shared-components.module';

@NgModule({
  declarations: [],
  imports: [SharedModule, SharedComponentsModule],
})
export class SubjectsModule {}
