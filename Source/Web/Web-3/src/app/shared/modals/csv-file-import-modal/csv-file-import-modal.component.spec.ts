import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CsvFileImportModalComponent } from './csv-file-import-modal.component';

describe('CsvFileImportModalComponent', (): void => {
  let component: CsvFileImportModalComponent;
  let fixture: ComponentFixture<CsvFileImportModalComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      declarations: [ CsvFileImportModalComponent ]
    })
    .compileComponents();
  });

  beforeEach((): void => {
    fixture = TestBed.createComponent(CsvFileImportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
