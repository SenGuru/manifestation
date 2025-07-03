import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigurationFileStorageEditorComponent } from './configuration-file-storage-editor.component';

describe('ConfigurationReportServerComponent', (): void => {
  let component: ConfigurationFileStorageEditorComponent;
  let fixture: ComponentFixture<ConfigurationFileStorageEditorComponent>;

  // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
  beforeEach(((): void => {
    TestBed.configureTestingModule({
      declarations: [ ConfigurationFileStorageEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(ConfigurationFileStorageEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
