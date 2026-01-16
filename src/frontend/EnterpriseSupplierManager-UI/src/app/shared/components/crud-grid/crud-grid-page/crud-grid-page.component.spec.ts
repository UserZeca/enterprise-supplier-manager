import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudGridComponent } from './crud-grid-page.component';

describe('CrudGridPageComponent', () => {
  let component: CrudGridComponent;
  let fixture: ComponentFixture<CrudGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CrudGridComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CrudGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
