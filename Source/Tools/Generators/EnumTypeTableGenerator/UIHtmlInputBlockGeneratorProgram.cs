using System;
using System.Collections.Generic;
using System.Text;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class UIHtmlInputBlockGeneratorProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Parameter should be <TableName> <PropertyName> <PropertyTypeName> <ThemeName>");
                return;
            }

            GetHtmlInputBlock(args[1], args[2], args[3], args[4]);
             
        }

        private static void GetHtmlInputBlock(string tableName, string propertyName, string propertyType, string theme)
        {
            var stringList = getInputTypes();

            if (!stringList.Contains(propertyType))
                throw new Exception($"{propertyType} is not a valid type");
             
            var propertyFirstLetterLowerCase = StringUtils.MakeFirstCharAsLower(propertyName);
            var propertyFirstLetterUpperCase = StringUtils.MakeFirstCharAsUpper(propertyName);
            var propertySplittedPascalName = StringUtils.SplitCamelCase(propertyFirstLetterUpperCase);
            var tableFirstNameUpperCase = StringUtils.MakeFirstCharAsUpper(tableName);
            var tableFirstNameLowerCase = StringUtils.MakeFirstCharAsLower(tableName);
            var tableSplittedPascalName = StringUtils.SplitCamelCase(tableFirstNameUpperCase);                
             
     
            if ((propertyType == "country"))
            {
                if (theme == "velzon")
                {

                    Console.Write($@"

                #################  IN HTML FILE  #######################   

              <div class=""col-sm-6"">
                  <label class=""form-label"" for=""selectedState"">State
                    <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                  </label>
                  <div *ngIf=""!isViewMode"">
                   <ng-select [items]=""states"" #vcUserState #lvUserState=""ngModel"" placeholder=""State""
                      [clearable]=""false"" class=""form-control""
                      [ngClass]=""{{ 'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lvState.invalid }}"" name=""selectedState""
                      id=""selectedState"" bindLabel=""StateName"" [(ngModel)]=""selectedState"" required>
                    </ng-select>
                    <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lvState.invalid"" class=""invalid-feedback invalid-feedback-fixed"">
                      <div *ngIf=""lvState.errors?.required"">State is required</div>
                    </div>
                  </div>
                  <div *ngIf=""isViewMode"">
                    {{{{{tableFirstNameLowerCase}Model?.stateName ? {tableFirstNameLowerCase}Model.stateName : '-'}}}}
                  </div>
                </div>
         
               <div class=""col-sm-6"">
                 <label class=""form-label"" for=""selectedCountry"">Country
                   <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                 </label>
                 <div *ngIf=""!isViewMode"">
                   <ng-select [items]=""countrys"" #vcCountry #lvCountry=""ngModel"" placeholder=""Country""
                     [clearable]=""false"" class=""form-control"" (change)=""onChangeCountry(selectedCountry)""
                     [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lvUserCountry.invalid }}"" name=""selectedCountry""
                     id=""selectedCountry"" bindLabel=""Country"" [(ngModel)]=""selectedCountry"" required>
                   </ng-select>
                   <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lvUserCountry.invalid"" class=""invalid-feedback invalid-feedback-fixed"">
                     <div *ngIf=""lvCountry.errors?.required"">Country is required</div>
                   </div>
                 </div>
                 <div *ngIf=""isViewMode"">
                   {{{{{tableFirstNameLowerCase}Model?.countryName ? {tableFirstNameLowerCase}Model.countryName : '-'}}}}
                 </div>
               </div>
                ");

                    Console.Write($@"
                       
                    ######################## IN TS FILE ##########################

                      public stateTypeahead$ = new Subject<string>();
                      public stateTypeaheadLoading: boolean = false;
                      public countryTypeahead$ = new Subject<string>();
                      public countryTypeaheadLoading: boolean = false;

                      stateSearchCriteria: StateSearchCriteria = new StateSearchCriteria();

                      states: StateModel[] = [];
                      selectedState: StateModel;
                      countrys: CountryModel[] = [];
                      selectedCountry: CountryModel;

                       /////////////////// in onInit Function //////////
                       
                        this.getCountrys('');

                                        
                        ////////////////////   in typeahead function  /////////////////////
                             
                   this.stateTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                      this.stateTypeaheadLoading = true;
                      const stateSearchCriteria = new StateSearchCriteria();
                      Object.assign(stateSearchCriteria, this.stateSearchCriteria);
                      stateSearchCriteria.keyword = searchValue;
                      if (this.selectedCountry) stateSearchCriteria.countryId = this.selectedCountry.countryId;
                      return this.httpService.getStates(stateSearchCriteria, getHeaderWithBlockUI());
                    }}), untilDestroyed(this)).subscribe({{
                      next: (data) => {{
                        this.stateTypeaheadLoading = false;
                        this.states = data.model;
                      }},
                      error: err => {{
                        this.stateTypeaheadLoading = false;
                      }},
                      complete: () => null,
                    }});

                    this.countryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                      this.countryTypeaheadLoading = true;
                      return this.httpService.getSupportedCountries(getHeaderWithBlockUI());
                    }}), untilDestroyed(this)).subscribe({{
                      next: (data) => {{
                        this.countryTypeaheadLoading = false;
                        this.countrys = data.model;
                      }},
                      error: err => {{
                        this.countryTypeaheadLoading = false;
                      }},
                      complete: () => null,
                    }});

                     ////////////////////  get api function  /////////////////////
                       getCountrys(keyword: string): void {{
                        this.httpService.getSupportedCountries().subscribe(result => {{
                          if (!result.isSuccess) return;
                          this.countrys = result.model;
                          if (this.countrys.length === 0) return;
                          if (!ObjectUtils.isNullOrUndefined(this.{tableFirstNameLowerCase}Model.countryId)) {{
                            this.selectedCountry = this.countrys.find(item => item.countryId == this.{tableFirstNameLowerCase}Model.countryId);
                            this.stateSearchCriteria.countryId = this.selectedCountry ? this.selectedCountry.countryId : null;
                            this.getStates(this.stateSearchCriteria);
                          }} else {{
                            this.selectedCountry = this.countrys.find(item => item.countryId == COUNTRY_ID_INDIA);
                            this.stateSearchCriteria.countryId = COUNTRY_ID_INDIA;
                            this.getStates(this.stateSearchCriteria);
                          }}
                        }});
                      }}
                        
                         onChangeCountry(selectedCountry: CountryModel): void {{
                            this.selectedState = null;
                            const stateSearchCriteria = new StateSearchCriteria();
                            stateSearchCriteria.countryId = selectedCountry.countryId;
                            this.getStates(stateSearchCriteria);
                          }}


                      getStates(stateSearchCriteria: StateSearchCriteria): void {{
                        this.httpService.getStates(stateSearchCriteria).subscribe({{
                          next: (result) => {{
                            if (!result.isSuccess) {{
                              this.dialogService.errorWithReturnModel('Not Able to get states', result);
                              return;
                            }}
                            this.states = result.model;
                            if (!this.isCreateMode) {{
                              this.selectedState = this.states.find(item => item.stateId == this.{tableFirstNameLowerCase}Model.stateId);
                            }}
                          }},
                          error: (err) => this.dialogService.error('Not Able to get states'),
                          complete: () => null,
                        }});
                      }}

                    //////////////////////////   In generate function ////////////////////
                    
                    this.{tableFirstNameLowerCase}Model.countryId = this.selectedCountry ? this.selectedCountry.countryId : null;
                    this.{tableFirstNameLowerCase}Model.stateId = this.selectedState ? this.selectedState.stateId : null;                    


                ");
                }
                else if (theme == "tabler")
                {


                    Console.Write($@"

                #################  IN HTML FILE  #######################   

              <div class=""col-sm-6"">
                  <label class=""form-label"" for=""selectedState"">State
                    <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                  </label>
                  <div *ngIf=""!isViewMode"">
                   <ng-select [items]=""states"" #vcUserState #lvUserState=""ngModel"" placeholder=""State""
                      [clearable]=""false"" [ngClass]=""{{ 'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lvState.invalid }}"" name=""selectedState""
                      id=""selectedState"" bindLabel=""StateName"" [(ngModel)]=""selectedState"" required>
                    </ng-select>
                    <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lvState.invalid"" class=""invalid-feedback invalid-feedback-fixed"">
                      <div *ngIf=""lvState.errors?.required"">State is required</div>
                    </div>
                  </div>
                  <div *ngIf=""isViewMode"">
                    {{{{{tableFirstNameLowerCase}Model?.stateName ? {tableFirstNameLowerCase}Model.stateName : '-'}}}}
                  </div>
                </div>
         
               <div class=""col-sm-6"">
                 <label class=""form-label"" for=""selectedCountry"">Country
                   <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                 </label>
                 <div *ngIf=""!isViewMode"">
                   <ng-select [items]=""countrys"" #vcCountry #lvCountry=""ngModel"" placeholder=""Country""
                     [clearable]=""false"" (change)=""onChangeCountry(selectedCountry)""
                     [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lvUserCountry.invalid }}"" name=""selectedCountry""
                     id=""selectedCountry"" bindLabel=""Country"" [(ngModel)]=""selectedCountry"" required>
                   </ng-select>
                   <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lvUserCountry.invalid"" class=""invalid-feedback invalid-feedback-fixed"">
                     <div *ngIf=""lvCountry.errors?.required"">Country is required</div>
                   </div>
                 </div>
                 <div *ngIf=""isViewMode"">
                   {{{{{tableFirstNameLowerCase}Model?.countryName ? {tableFirstNameLowerCase}Model.countryName : '-'}}}}
                 </div>
               </div>
                ");

                    Console.Write($@"
                       
                    ######################## IN TS FILE ##########################

                      public stateTypeahead$ = new Subject<string>();
                      public stateTypeaheadLoading: boolean = false;
                      public countryTypeahead$ = new Subject<string>();
                      public countryTypeaheadLoading: boolean = false;

                      stateSearchCriteria: StateSearchCriteria = new StateSearchCriteria();

                      states: StateModel[] = [];
                      selectedState: StateModel;
                      countrys: CountryModel[] = [];
                      selectedCountry: CountryModel;

                       /////////////////// in onInit Function //////////
                       
                        this.getCountrys('');

                                        
                        ////////////////////   in typeahead function  /////////////////////
                             
                   this.stateTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                      this.stateTypeaheadLoading = true;
                      const stateSearchCriteria = new StateSearchCriteria();
                      Object.assign(stateSearchCriteria, this.stateSearchCriteria);
                      stateSearchCriteria.keyword = searchValue;
                      if (this.selectedCountry) stateSearchCriteria.countryId = this.selectedCountry.countryId;
                      return this.httpService.getStates(stateSearchCriteria, getHeaderWithBlockUI());
                    }}), untilDestroyed(this)).subscribe({{
                      next: (data) => {{
                        this.stateTypeaheadLoading = false;
                        this.states = data.model;
                      }},
                      error: err => {{
                        this.stateTypeaheadLoading = false;
                      }},
                      complete: () => null,
                    }});

                    this.countryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                      this.countryTypeaheadLoading = true;
                      return this.httpService.getSupportedCountries(getHeaderWithBlockUI());
                    }}), untilDestroyed(this)).subscribe({{
                      next: (data) => {{
                        this.countryTypeaheadLoading = false;
                        this.countrys = data.model;
                      }},
                      error: err => {{
                        this.countryTypeaheadLoading = false;
                      }},
                      complete: () => null,
                    }});

                     ////////////////////  get api function  /////////////////////
                       getCountrys(keyword: string): void {{
                        this.httpService.getSupportedCountries().subscribe(result => {{
                          if (!result.isSuccess) return;
                          this.countrys = result.model;
                          if (this.countrys.length === 0) return;
                          if (!ObjectUtils.isNullOrUndefined(this.{tableFirstNameLowerCase}Model.countryId)) {{
                            this.selectedCountry = this.countrys.find(item => item.countryId == this.{tableFirstNameLowerCase}Model.countryId);
                            this.stateSearchCriteria.countryId = this.selectedCountry ? this.selectedCountry.countryId : null;
                            this.getStates(this.stateSearchCriteria);
                          }} else {{
                            this.selectedCountry = this.countrys.find(item => item.countryId == COUNTRY_ID_INDIA);
                            this.stateSearchCriteria.countryId = COUNTRY_ID_INDIA;
                            this.getStates(this.stateSearchCriteria);
                          }}
                        }});
                      }}
                        
                         onChangeCountry(selectedCountry: CountryModel): void {{
                            this.selectedState = null;
                            const stateSearchCriteria = new StateSearchCriteria();
                            stateSearchCriteria.countryId = selectedCountry.countryId;
                            this.getStates(stateSearchCriteria);
                          }}


                      getStates(stateSearchCriteria: StateSearchCriteria): void {{
                        this.httpService.getStates(stateSearchCriteria).subscribe({{
                          next: (result) => {{
                            if (!result.isSuccess) {{
                              this.dialogService.errorWithReturnModel('Not Able to get states', result);
                              return;
                            }}
                            this.states = result.model;
                            if (!this.isCreateMode) {{
                              this.selectedState = this.states.find(item => item.stateId == this.{tableFirstNameLowerCase}Model.stateId);
                            }}
                          }},
                          error: (err) => this.dialogService.error('Not Able to get states'),
                          complete: () => null,
                        }});
                      }}

                    //////////////////////////   In generate function ////////////////////
                    
                    this.{tableFirstNameLowerCase}Model.countryId = this.selectedCountry ? this.selectedCountry.countryId : null;
                    this.{tableFirstNameLowerCase}Model.stateId = this.selectedState ? this.selectedState.stateId : null;                    


                ");
                }
                else
                {
                    throw new Exception("Unknow theme provided");
                }

            }
            else if (propertyType == "date")
            {
                if (theme == "velzon")
                {
                    Console.Write($@"
                       ###############  IN HTML FILE  #######################

                       <div class=""col-sm-6"">
                         <label class=""form-label"" for=""{propertyFirstLetterLowerCase}""> {propertySplittedPascalName}
                           <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                         </label>
                         <div *ngIf=""!isViewMode"">
                           <input class=""form-control flatpickr-input"" type=""text"" mwlFlatpickr #lv{propertyFirstLetterUpperCase}=""ngModel""
                             name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" [altInput]=""true""
                             [convertModelValue]=""true"" required altFormat=""F j, Y"" dateFormat=""Y-m-d""
                             [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                             placeholder=""Select {propertySplittedPascalName}"" />
                           <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                             class=""invalid-feedback invalid-feedback-fixed"">
                             <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required"">{propertySplittedPascalName} is required</div>
                           </div>
                         </div>
                         <div *ngIf=""isViewMode"">
                           {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? ({tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} | date : 'MM/dd/yy')  : '-'}}}}
                         </div>
                       </div>
 
                ");
                }
                else if (theme == "tabler")
                {
                    Console.Write($@"
                       ###############  IN HTML FILE  #######################

                       <div class=""col-sm-6"">
                         <label class=""form-label"" for=""{propertyFirstLetterLowerCase}""> {propertySplittedPascalName}
                           <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                         </label>
                         <div *ngIf=""!isViewMode"">
                           <input  type=""text"" class=""form-control flatpickr-input"" mwlFlatpickr #lv{propertyFirstLetterUpperCase}=""ngModel""
                             name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" [altInput]=""true""
                             [convertModelValue]=""true"" required altFormat=""F j, Y"" dateFormat=""Y-m-d""
                             [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                             placeholder=""Select {propertySplittedPascalName}"" />
                           <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                             class=""invalid-feedback invalid-feedback-fixed"">
                             <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required"">{propertySplittedPascalName} is required</div>
                           </div>
                         </div>
                         <div *ngIf=""isViewMode"">
                           {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? ({tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} | date : 'MM/dd/yy')  : '-'}}}}
                         </div>
                       </div> 
                ");
                }
                else
                {
                    throw new Exception("Unknow theme provided");
                }
            }
            else if(propertyType ==  "time")
            {
                if (theme == "velzon")
                {
                    Console.Write($@"

                       ###############  IN HTML FILE  #######################   

                     <div class=""col-cd-1-12"">
                        <label class=""form-label"" for=""{propertyFirstLetterLowerCase}""> {propertySplittedPascalName}
                          <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                        </label>
                        <div *ngIf=""!isViewMode"">
                          <input type=""time"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required
                            class=""form-control"" [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                            placeholder=""{propertySplittedPascalName}"">
                          <div class=""invalid-feedback invalid-feedback-fixed"">
                            <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                              {propertySplittedPascalName} is required
                            </span>
                          </div>
                        </div>
                        <div *ngIf=""isViewMode"">
                          {{{{ {tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? ({tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} | secondsToHour) : '-' }}}}
                        </div>
                      </div>

                                        
                        ####################  IN TS FILE #############################

                        {propertyFirstLetterLowerCase}: string;

                         ///// onInit function ///////////////
                         
                         this.{propertyFirstLetterLowerCase} = this.timeConversionService.getFormattedTime(this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase});

                        ////  generate function //////////////
                        
                        this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} = this.timeConversionService.convertTimeToSeconds(this.{propertyFirstLetterLowerCase});

                       ");
                }
                else if (theme == "tabler")
                {

                    Console.Write($@"

                       ###############  IN HTML FILE  #######################   

                     <div class=""col-cd-1-12"">
                        <label class=""form-label"" for=""{propertyFirstLetterLowerCase}""> {propertySplittedPascalName}
                          <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                        </label>
                        <div *ngIf=""!isViewMode"">
                          <input type=""time"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required
                            [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                            placeholder=""{propertySplittedPascalName}"">
                          <div class=""invalid-feedback invalid-feedback-fixed"">
                            <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                              {propertySplittedPascalName} is required
                            </span>
                          </div>
                        </div>
                        <div *ngIf=""isViewMode"">
                          {{{{ {tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? ({tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} | secondsToHour) : '-' }}}}
                        </div>
                      </div>

                                        
                        ####################  IN TS FILE #############################

                        {propertyFirstLetterLowerCase}: string;

                         ///// onInit function ///////////////
                         
                         this.{propertyFirstLetterLowerCase} = this.timeConversionService.getFormattedTime(this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase});

                        ////  generate function //////////////
                        
                        this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} = this.timeConversionService.convertTimeToSeconds(this.{propertyFirstLetterLowerCase});

                       ");
                }
                else
                {
                    throw new Exception("Unknow theme provided");
                }
            }
            else if (propertyType == "checkbox")
            {
                if (theme == "velzon")
                {
                    Console.Write($@"
                
                        ###############  IN HTML FILE  #######################

                         <div class=""col-sm-6"">
                            <div class=""mt-3"">
                              <label class=""form-label"" for=""is{propertyFirstLetterUpperCase}"">
                              </label>
                              <div class=""form-check"">
                                <input class=""form-check-input"" type=""checkbox"" value=""true"" name=""is{propertyFirstLetterUpperCase}""
                                  id=""is{propertyFirstLetterUpperCase}"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" [disabled]=""isViewMode"">
                                <label class=""form-check-label"" for=""is{propertyFirstLetterUpperCase}"">
                                  {propertySplittedPascalName}
                                </label>
                              </div>
                            </div>
                          </div>      

                        ");

                }
                else if (theme == "tabler")
                {
                    Console.Write($@"
                
                        ###############  IN HTML FILE  #######################

                         <div class=""col-sm-6"">
                            <div class=""mt-3"">
                              <label class=""form-label"" for=""is{propertyFirstLetterUpperCase}"">
                              </label>
                              <div class=""form-check"">
                                <input class=""form-check-input"" type=""checkbox"" value=""true"" name=""is{propertyFirstLetterUpperCase}""
                                  id=""is{propertyFirstLetterUpperCase}"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" [disabled]=""isViewMode"">
                                <label class=""form-check-label"" for=""is{propertyFirstLetterUpperCase}"">
                                  {propertySplittedPascalName}
                                </label>
                              </div>
                            </div>
                          </div>                

                        ");
                }
                else
                {
                    throw new Exception("Unknow theme provided");
                }
            } 
            else if (propertyType == "predefinedSingleSelect")
            {
                if (theme == "velzon")
                {
                    Console.Write($@"

                        ###############  IN HTML FILE  #######################

                        <div class=""col-sm-6"">
                          <label class=""form-label"" for=""selected{propertyFirstLetterUpperCase}""> {propertySplittedPascalName}
                            <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                          </label>
                          <div *ngIf=""!isViewMode"">
                            <ng-select [items]=""{propertyFirstLetterLowerCase}s"" #lv{propertyFirstLetterUpperCase}=""ngModel"" placeholder=""Select {propertySplittedPascalName}""
                              [clearable]=""false"" class=""form-control""
                              [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                              name=""selected{propertyFirstLetterUpperCase}"" id=""selected{propertyFirstLetterUpperCase}"" bindLabel=""type""
                              [(ngModel)]=""selected{propertyFirstLetterUpperCase}"" required>
                            </ng-select>
                            <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                              class=""invalid-feedback invalid-feedback-fixed"">
                              <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required""> {propertySplittedPascalName} is required</div>
                            </div>
                          </div>
                          <div *ngIf=""isViewMode"">
                            {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase}Name ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Name :
                            '-'}}}}
                          </div>
                        </div>

                        ######################## IN TS FILE ##########################

                      ///////////////// member initialization ///////////////
                      
                            
                      {propertyFirstLetterLowerCase}s: Status[] = [];
                      selected{propertyFirstLetterUpperCase}: Status;

 
                       /////////////////// in onInit Function //////////
                       

                       this.{propertyFirstLetterLowerCase}s = this.selectService.Get{propertyFirstLetterUpperCase}s();
                       if(!this.isCreateMode) this.selected{propertyFirstLetterUpperCase} = this.{propertyFirstLetterLowerCase}s.find(item => item.id == this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id);


                                        
                        ////////////////////   in generate function  /////////////////////
                             
                      this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id = this.selected{propertyFirstLetterUpperCase} ? this.selected{propertyFirstLetterUpperCase}.id : null;

                ");
                }
                else if (theme == "tabler")
                {
                    Console.Write($@"

                        ###############  IN HTML FILE  #######################

                        <div class=""col-sm-6"">
                          <label class=""form-label"" for=""selected{propertyFirstLetterUpperCase}""> {propertySplittedPascalName}
                            <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                          </label>
                          <div *ngIf=""!isViewMode"">
                            <ng-select [items]=""{propertyFirstLetterLowerCase}s"" #lv{propertyFirstLetterUpperCase}=""ngModel"" placeholder=""Select {propertySplittedPascalName}""
                              [clearable]=""false"" [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                              name=""selected{propertyFirstLetterUpperCase}"" id=""selected{propertyFirstLetterUpperCase}"" bindLabel=""type""
                              [(ngModel)]=""selected{propertyFirstLetterUpperCase}"" required>
                            </ng-select>
                            <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                              class=""invalid-feedback invalid-feedback-fixed"">
                              <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required""> {propertySplittedPascalName} is required</div>
                            </div>
                          </div>
                          <div *ngIf=""isViewMode"">
                            {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase}Name ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Name :
                            '-'}}}}
                          </div>
                        </div>

                        ######################## IN TS FILE ##########################

                      ///////////////// member initialization ///////////////
                      
                            
                      {propertyFirstLetterLowerCase}s: Status[] = [];
                      selected{propertyFirstLetterUpperCase}: Status;

 
                       /////////////////// in onInit Function //////////
                       

                       this.{propertyFirstLetterLowerCase}s = this.selectService.Get{propertyFirstLetterUpperCase}s();
                       if(!this.isCreateMode) this.selected{propertyFirstLetterUpperCase} = this.{propertyFirstLetterLowerCase}s.find(item => item.id == this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id);


                                        
                        ////////////////////   in generate function  /////////////////////
                             
                      this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id = this.selected{propertyFirstLetterUpperCase} ? this.selected{propertyFirstLetterUpperCase}.id : null;

                ");
                }
                else
                {
                    throw new Exception("Unknow theme provided");
                }

            }
            else if (propertyType == "singleSelect")
                    {
                if (theme == "velzon")
                {
                    Console.Write($@"
                             <div class=""col-sm-6"">
                                      <label class=""form-label"" for=""selected{propertyFirstLetterUpperCase}"">{propertySplittedPascalName}
                                        <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                                         <span class=""pull-right cursor-pointer mr-1"" *ngIf=""!isViewMode"" (click)=""showAdd{propertyFirstLetterUpperCase}Modal()""><i
                                           class=""ri-add-line""></i> Add</span>
                                      </label>
                                      <div *ngIf=""!isViewMode"">
                                        <ng-select [items]=""{propertyFirstLetterLowerCase}s"" #lv{propertyFirstLetterUpperCase}=""ngModel""
                                          placeholder=""{propertySplittedPascalName}"" [clearable]=""false"" class=""form-control""
                                          [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                                          name=""selected{propertyFirstLetterUpperCase}"" id=""selected{propertyFirstLetterUpperCase}"" bindLabel=""{propertyFirstLetterLowerCase}Name""
                                          [(ngModel)]=""selected{propertyFirstLetterUpperCase}"" required [typeahead]=""{propertyFirstLetterLowerCase}Typeahead$""
                                          [loading]=""{propertyFirstLetterLowerCase}TypeaheadLoading"">
                                        </ng-select>
                                        <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                                          class=""invalid-feedback invalid-feedback-fixed"">
                                          <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required"">{propertySplittedPascalName} is required</div>
                                        </div>
                                      </div>
                                      <div *ngIf=""isViewMode"">
                                        {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase}Name ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Name :
                                     '-'}}}}
                                      </div>
                                    </div>


                                    ######################## IN TS FILE ##########################

                                  ///////////////// member initialization ///////////////
                      
                                  {propertyFirstLetterLowerCase}SearchCriteria: {propertyFirstLetterUpperCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();

                                  public {propertyFirstLetterLowerCase}Typeahead$ = new Subject<string>();
                                  public {propertyFirstLetterLowerCase}TypeaheadLoading: boolean = false;
                                  {propertyFirstLetterLowerCase}s: {propertyFirstLetterUpperCase}Model[] = [];
                                  selected{propertyFirstLetterUpperCase}: {propertyFirstLetterUpperCase}Model;
 
                                   /////////////////// in onInit Function //////////
                       
                                    this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria);

                                  ////////////////////  In Typeahead Function //////////////
                                  
                                   
                                          this.{propertyFirstLetterLowerCase}Typeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                                          this.{propertyFirstLetterLowerCase}TypeaheadLoading = true;
                                          const {propertyFirstLetterLowerCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();
                                          {propertyFirstLetterLowerCase}SearchCriteria.keyword = searchValue;
                                          {propertyFirstLetterLowerCase}SearchCriteria.companyId = this.companyId;
                                          if(!this.isCreateMode) {propertyFirstLetterLowerCase}SearchCriteria.selectedId = this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id;
                                          return this.httpService.get{propertyFirstLetterUpperCase}s({propertyFirstLetterLowerCase}SearchCriteria, getHeaderWithBlockUI());
                                        }}), untilDestroyed(this)).subscribe({{
                                          next: (data) => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                            this.{propertyFirstLetterLowerCase}s = data.model;
                                          }},
                                          error: err => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                          }},
                                          complete: () => null,
                                        }});
                                        
                                    ////////////////////   in get Function  ///////////////////////
                                    
                                        showAdd{propertyFirstLetterUpperCase}Modal(): void {{
                                          const modalRef = this.modalService.open({propertyFirstLetterUpperCase}CrudModalComponent, {{ windowClass: 'my-class', size: 'xl', beforeDismiss: () => false }});
                                          const componentInstance = modalRef.componentInstance as {propertyFirstLetterUpperCase}CrudModalComponent;
                                          componentInstance.isCreateMode = true;
                                          componentInstance.buttonText = 'Create';
                                          componentInstance.title = 'Create {propertySplittedPascalName}';
                                          componentInstance.isSuperAdmin = this.isSuperAdmin;
                                          componentInstance.companyId = this.companyId;
                                          componentInstance.companyName = this.companyName;
                                          componentInstance.layoutTypeId = this.layoutTypeId;
                                          componentInstance.onCloseCallback = (res: {propertyFirstLetterUpperCase}Model): void => {{
                                            if (ObjectUtils.isObjNullOrUndefined(res)) return;
                                            this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria, res.{propertyFirstLetterLowerCase}Id);
                                          }}
                                        }}

                                         get{propertyFirstLetterUpperCase}s(searchModel: {propertyFirstLetterUpperCase}SearchCriteria, selected{propertyFirstLetterUpperCase}Id: number = null): void {{
                                          searchModel.sortAscending = false;
                                          searchModel.sortBy = 'createdDate';
                                          searchModel.companyId = this.companyId;
                                          if(!this.isCreateMode) searchModel.selectedId = this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id;
                                          this.httpService.get{propertyFirstLetterUpperCase}s(searchModel).subscribe({{
                                            next: (data) => {{
                                              if (!data.isSuccess)
                                                return;
                                              this.{propertyFirstLetterLowerCase}s = data.model;
                                              if (!this.isCreateMode) this.selected{propertyFirstLetterUpperCase} = data.model.find(item => item.{propertyFirstLetterLowerCase}Id === this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id);

                                              if (selected{propertyFirstLetterUpperCase}Id) this.selected{propertyFirstLetterUpperCase} = data.model.find(item => item.{propertyFirstLetterLowerCase}Id === selected{propertyFirstLetterUpperCase}Id);
                                            }},
                                            error: (error) => this.dialogService.error('Not Able to get {propertySplittedPascalName}s'),
                                            complete: () => null,
                                          }});
                                        }}
    
                               /////////////////////////// generate function /////////////////////////////
                               
                                this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id = this.selected{propertyFirstLetterUpperCase} ? this.selected{propertyFirstLetterUpperCase}.{propertyFirstLetterLowerCase}Id : null;

                                                    
                            ");
                }
                else if (theme == "tabler")
                {

                    Console.Write($@"
                             <div class=""col-sm-6"">
                                      <label class=""form-label"" for=""selected{propertyFirstLetterUpperCase}"">{propertySplittedPascalName}
                                        <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                                         <span class=""pull-right cursor-pointer mr-1"" *ngIf=""!isViewMode"" (click)=""showAdd{propertyFirstLetterUpperCase}Modal()""><i
                                           class=""ri-add-line""></i> Add</span>
                                      </label>
                                      <div *ngIf=""!isViewMode"">
                                        <ng-select [items]=""{propertyFirstLetterLowerCase}s"" #lv{propertyFirstLetterUpperCase}=""ngModel""
                                          placeholder=""{propertySplittedPascalName}"" [clearable]=""false"" [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                                          name=""selected{propertyFirstLetterUpperCase}"" id=""selected{propertyFirstLetterUpperCase}"" bindLabel=""{propertyFirstLetterLowerCase}Name""
                                          [(ngModel)]=""selected{propertyFirstLetterUpperCase}"" required [typeahead]=""{propertyFirstLetterLowerCase}Typeahead$""
                                          [loading]=""{propertyFirstLetterLowerCase}TypeaheadLoading"">
                                        </ng-select>
                                        <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                                          class=""invalid-feedback invalid-feedback-fixed"">
                                          <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required"">{propertySplittedPascalName} is required</div>
                                        </div>
                                      </div>
                                      <div *ngIf=""isViewMode"">
                                        {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase}Name ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Name :
                                     '-'}}}}
                                      </div>
                                    </div>


                                    ######################## IN TS FILE ##########################

                                  ///////////////// member initialization ///////////////
                      
                                  {propertyFirstLetterLowerCase}SearchCriteria: {propertyFirstLetterUpperCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();

                                  public {propertyFirstLetterLowerCase}Typeahead$ = new Subject<string>();
                                  public {propertyFirstLetterLowerCase}TypeaheadLoading: boolean = false;
                                  {propertyFirstLetterLowerCase}s: {propertyFirstLetterUpperCase}Model[] = [];
                                  selected{propertyFirstLetterUpperCase}: {propertyFirstLetterUpperCase}Model;
 
                                   /////////////////// in onInit Function //////////
                       
                                    this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria);

                                  ////////////////////  In Typeahead Function //////////////
                                  
                                   
                                          this.{propertyFirstLetterLowerCase}Typeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                                          this.{propertyFirstLetterLowerCase}TypeaheadLoading = true;
                                          const {propertyFirstLetterLowerCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();
                                          {propertyFirstLetterLowerCase}SearchCriteria.keyword = searchValue;
                                          {propertyFirstLetterLowerCase}SearchCriteria.companyId = this.companyId;
                                          if(!this.isCreateMode) {propertyFirstLetterLowerCase}SearchCriteria.selectedId = this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id;
                                          return this.httpService.get{propertyFirstLetterUpperCase}s({propertyFirstLetterLowerCase}SearchCriteria, getHeaderWithBlockUI());
                                        }}), untilDestroyed(this)).subscribe({{
                                          next: (data) => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                            this.{propertyFirstLetterLowerCase}s = data.model;
                                          }},
                                          error: err => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                          }},
                                          complete: () => null,
                                        }});
                                        
                                    ////////////////////   in get Function  ///////////////////////
                                    
                                        showAdd{propertyFirstLetterUpperCase}Modal(): void {{
                                          const modalRef = this.modalService.open({propertyFirstLetterUpperCase}CrudModalComponent, {{ windowClass: 'my-class', size: 'xl', beforeDismiss: () => false }});
                                          const componentInstance = modalRef.componentInstance as {propertyFirstLetterUpperCase}CrudModalComponent;
                                          componentInstance.isCreateMode = true;
                                          componentInstance.buttonText = 'Create';
                                          componentInstance.title = 'Create {propertySplittedPascalName}';
                                          componentInstance.isSuperAdmin = this.isSuperAdmin;
                                          componentInstance.companyId = this.companyId;
                                          componentInstance.companyName = this.companyName;
                                          componentInstance.layoutTypeId = this.layoutTypeId;
                                          componentInstance.onCloseCallback = (res: {propertyFirstLetterUpperCase}Model): void => {{
                                            if (ObjectUtils.isObjNullOrUndefined(res)) return;
                                            this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria, res.{propertyFirstLetterLowerCase}Id);
                                          }}
                                        }}

                                         get{propertyFirstLetterUpperCase}s(searchModel: {propertyFirstLetterUpperCase}SearchCriteria, selected{propertyFirstLetterUpperCase}Id: number = null): void {{
                                          searchModel.sortAscending = false;
                                          searchModel.sortBy = 'createdDate';
                                          searchModel.companyId = this.companyId;
                                          if(!this.isCreateMode) searchModel.selectedId = this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id;
                                          this.httpService.get{propertyFirstLetterUpperCase}s(searchModel).subscribe({{
                                            next: (data) => {{
                                              if (!data.isSuccess)
                                                return;
                                              this.{propertyFirstLetterLowerCase}s = data.model;
                                              if (!this.isCreateMode) this.selected{propertyFirstLetterUpperCase} = data.model.find(item => item.{propertyFirstLetterLowerCase}Id === this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id);

                                              if (selected{propertyFirstLetterUpperCase}Id) this.selected{propertyFirstLetterUpperCase} = data.model.find(item => item.{propertyFirstLetterLowerCase}Id === selected{propertyFirstLetterUpperCase}Id);
                                            }},
                                            error: (error) => this.dialogService.error('Not Able to get {propertySplittedPascalName}s'),
                                            complete: () => null,
                                          }});
                                        }}
    
                               /////////////////////////// generate function /////////////////////////////
                               
                                this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id = this.selected{propertyFirstLetterUpperCase} ? this.selected{propertyFirstLetterUpperCase}.{propertyFirstLetterLowerCase}Id : null;

                                                    
                            ");
                }
                else
                {
                    throw new Exception("Unknow theme provided");
                }
            }
            else if (propertyType == "multiSelect")
            {
                if (theme == "velzon")
                {
                    Console.Write($@"
                             <div class=""col-sm-6"">
                                      <label class=""form-label"" for=""selected{propertyFirstLetterUpperCase}s"" >{propertySplittedPascalName}
                                        <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                                          <span class=""pull-right cursor-pointer mr-1"" *ngIf=""!isViewMode"" (click)=""showAdd{propertyFirstLetterUpperCase}Modal()""><i
                                           class=""ri-add-line""></i> Add</span>
                                      </label>
                                      <div *ngIf=""!isViewMode"">
                                        <ng-select [items]=""{propertyFirstLetterLowerCase}s"" #lv{propertyFirstLetterUpperCase}=""ngModel""
                                          placeholder=""{propertySplittedPascalName}"" [clearable]=""false"" class=""form-control""  [multiple]=""true""
                                          [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                                          name=""selected{propertyFirstLetterUpperCase}s"" id=""selected{propertyFirstLetterUpperCase}s"" bindLabel=""{propertyFirstLetterLowerCase}Name""
                                          [(ngModel)]=""selected{propertyFirstLetterUpperCase}s"" required [typeahead]=""{propertyFirstLetterLowerCase}Typeahead$""
                                          [loading]=""{propertyFirstLetterLowerCase}TypeaheadLoading"">
                                        </ng-select>
                                        <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                                          class=""invalid-feedback invalid-feedback-fixed"">
                                          <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required"">{propertySplittedPascalName} is required</div>
                                        </div>
                                      </div>
                                      <div *ngIf=""isViewMode"">
                                         <span *ngFor=""let item of selected{tableFirstNameUpperCase}s"">{{{{item.{propertyFirstLetterLowerCase}Name}}}}</span>
                                         <span *ngIf=""selected{tableFirstNameUpperCase}s.length == 0""> - </span>
                                      </div>
                                    </div>


                                    ######################## IN TS FILE ##########################

                                  ///////////////// member initialization ///////////////
                      
                                  {propertyFirstLetterLowerCase}SearchCriteria: {propertyFirstLetterUpperCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();

                                  public {propertyFirstLetterLowerCase}Typeahead$ = new Subject<string>();
                                  public {propertyFirstLetterLowerCase}TypeaheadLoading: boolean = false;
                                  {propertyFirstLetterLowerCase}s: {propertyFirstLetterUpperCase}Model[] = [];
                                  selected{propertyFirstLetterUpperCase}s: {propertyFirstLetterUpperCase}Model[] = [];
 
                                   /////////////////// in onInit Function //////////
                       
                                    this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria);

                                  ////////////////////  In Typeahead Function //////////////
                                  
                                   
                                          this.{propertyFirstLetterLowerCase}Typeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                                          this.{propertyFirstLetterLowerCase}TypeaheadLoading = true;
                                          const {propertyFirstLetterLowerCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();
                                          {propertyFirstLetterLowerCase}SearchCriteria.keyword = searchValue;
                                          {propertyFirstLetterLowerCase}SearchCriteria.companyId = this.companyId;
                                          return this.httpService.get{propertyFirstLetterUpperCase}s({propertyFirstLetterLowerCase}SearchCriteria, getHeaderWithBlockUI());
                                        }}), untilDestroyed(this)).subscribe({{
                                          next: (data) => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                            this.{propertyFirstLetterLowerCase}s = data.model;
                                          }},
                                          error: err => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                          }},
                                          complete: () => null,
                                        }});
                                        
                                    ////////////////////   in get Function  ///////////////////////
                                    
                                        showAdd{propertyFirstLetterUpperCase}Modal(): void {{
                                          const modalRef = this.modalService.open({propertyFirstLetterUpperCase}CrudModalComponent, {{ windowClass: 'my-class', size: 'xl', beforeDismiss: () => false }});
                                          const componentInstance = modalRef.componentInstance as {propertyFirstLetterUpperCase}CrudModalComponent;
                                          componentInstance.isCreateMode = true;
                                          componentInstance.buttonText = 'Create';
                                          componentInstance.title = 'Create {propertySplittedPascalName}';
                                          componentInstance.isSuperAdmin = this.isSuperAdmin;
                                          componentInstance.companyId = this.companyId;
                                          componentInstance.companyName = this.companyName;
                                          componentInstance.layoutTypeId = this.layoutTypeId;
                                          componentInstance.onCloseCallback = (res: {propertyFirstLetterUpperCase}Model) => {{
                                            if (ObjectUtils.isObjNullOrUndefined(res)) return;
                                            this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria, res.{propertyFirstLetterLowerCase}Id);
                                          }}
                                        }}

                                         get{propertyFirstLetterUpperCase}s(searchModel: {propertyFirstLetterUpperCase}SearchCriteria, selected{propertyFirstLetterUpperCase}Id: number = null): void {{
                                          searchModel.sortAscending = false;
                                          searchModel.sortBy = 'createdDate';
                                          searchModel.companyId = this.companyId;
                                          this.httpService.get{propertyFirstLetterUpperCase}s(searchModel).subscribe({{
                                            next: (data) => {{
                                              if (!data.isSuccess)
                                                return;
                                              this.{propertyFirstLetterLowerCase}s = data.model;
                                              if (!this.isCreateMode) this.selected{propertyFirstLetterUpperCase}s = this.{propertyFirstLetterLowerCase}s.find(item => item.{propertyFirstLetterLowerCase}Id === this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id);

                                              if (selected{propertyFirstLetterUpperCase}Id) {{
                                                     const selectedItem = this.{propertyFirstLetterLowerCase}s.find(item => item.{propertyFirstLetterLowerCase}Id === selected{propertyFirstLetterUpperCase}Id);
                                                     this.selected{propertyFirstLetterUpperCase}s.push(selectedItem);
                                                }} 
                                            }},
                                            error: (error) => this.dialogService.error('Not Able to get {propertySplittedPascalName}s'),
                                            complete: () => null,
                                          }});
                                        }}
    
                               /////////////////////////// location /////////////////////////////
                               
                                this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Ids = this.selected{propertyFirstLetterLowerCase}s?.map(item => item.{propertyFirstLetterLowerCase}Id);

                            ");
                }
                else if (theme == "tabler")
                {

                    Console.Write($@"
                             <div class=""col-sm-6"">
                                      <label class=""form-label"" for=""selected{propertyFirstLetterUpperCase}s"" >{propertySplittedPascalName}
                                        <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                                          <span class=""pull-right cursor-pointer mr-1"" *ngIf=""!isViewMode"" (click)=""showAdd{propertyFirstLetterUpperCase}Modal()""><i
                                           class=""ri-add-line""></i> Add</span>
                                      </label>
                                      <div *ngIf=""!isViewMode"">
                                        <ng-select [items]=""{propertyFirstLetterLowerCase}s"" #lv{propertyFirstLetterUpperCase}=""ngModel""
                                          placeholder=""{propertySplittedPascalName}"" [clearable]=""false"" [multiple]=""true""
                                          [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                                          name=""selected{propertyFirstLetterUpperCase}s"" id=""selected{propertyFirstLetterUpperCase}s"" bindLabel=""{propertyFirstLetterLowerCase}Name""
                                          [(ngModel)]=""selected{propertyFirstLetterUpperCase}s"" required [typeahead]=""{propertyFirstLetterLowerCase}Typeahead$""
                                          [loading]=""{propertyFirstLetterLowerCase}TypeaheadLoading"">
                                        </ng-select>
                                        <div *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid""
                                          class=""invalid-feedback invalid-feedback-fixed"">
                                          <div *ngIf=""lv{propertyFirstLetterUpperCase}.errors?.required"">{propertySplittedPascalName} is required</div>
                                        </div>
                                      </div>
                                      <div *ngIf=""isViewMode"">
                                         <span *ngFor=""let item of selected{tableFirstNameUpperCase}s"">{{{{item.{propertyFirstLetterLowerCase}Name}}}}</span>
                                         <span *ngIf=""selected{tableFirstNameUpperCase}s.length == 0""> - </span>
                                      </div>
                                    </div>


                                    ######################## IN TS FILE ##########################

                                  ///////////////// member initialization ///////////////
                      
                                  {propertyFirstLetterLowerCase}SearchCriteria: {propertyFirstLetterUpperCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();

                                  public {propertyFirstLetterLowerCase}Typeahead$ = new Subject<string>();
                                  public {propertyFirstLetterLowerCase}TypeaheadLoading: boolean = false;
                                  {propertyFirstLetterLowerCase}s: {propertyFirstLetterUpperCase}Model[] = [];
                                  selected{propertyFirstLetterUpperCase}s: {propertyFirstLetterUpperCase}Model[] = [];
 
                                   /////////////////// in onInit Function //////////
                       
                                    this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria);

                                  ////////////////////  In Typeahead Function //////////////
                                  
                                   
                                          this.{propertyFirstLetterLowerCase}Typeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {{
                                          this.{propertyFirstLetterLowerCase}TypeaheadLoading = true;
                                          const {propertyFirstLetterLowerCase}SearchCriteria = new {propertyFirstLetterUpperCase}SearchCriteria();
                                          {propertyFirstLetterLowerCase}SearchCriteria.keyword = searchValue;
                                          {propertyFirstLetterLowerCase}SearchCriteria.companyId = this.companyId;
                                          return this.httpService.get{propertyFirstLetterUpperCase}s({propertyFirstLetterLowerCase}SearchCriteria, getHeaderWithBlockUI());
                                        }}), untilDestroyed(this)).subscribe({{
                                          next: (data) => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                            this.{propertyFirstLetterLowerCase}s = data.model;
                                          }},
                                          error: err => {{
                                            this.{propertyFirstLetterLowerCase}TypeaheadLoading = false;
                                          }},
                                          complete: () => null,
                                        }});
                                        
                                    ////////////////////   in get Function  ///////////////////////
                                    
                                        showAdd{propertyFirstLetterUpperCase}Modal(): void {{
                                          const modalRef = this.modalService.open({propertyFirstLetterUpperCase}CrudModalComponent, {{ windowClass: 'my-class', size: 'xl', beforeDismiss: () => false }});
                                          const componentInstance = modalRef.componentInstance as {propertyFirstLetterUpperCase}CrudModalComponent;
                                          componentInstance.isCreateMode = true;
                                          componentInstance.buttonText = 'Create';
                                          componentInstance.title = 'Create {propertySplittedPascalName}';
                                          componentInstance.isSuperAdmin = this.isSuperAdmin;
                                          componentInstance.companyId = this.companyId;
                                          componentInstance.companyName = this.companyName;
                                          componentInstance.layoutTypeId = this.layoutTypeId;
                                          componentInstance.onCloseCallback = (res: {propertyFirstLetterUpperCase}Model) => {{
                                            if (ObjectUtils.isObjNullOrUndefined(res)) return;
                                            this.get{propertyFirstLetterUpperCase}s(this.{propertyFirstLetterLowerCase}SearchCriteria, res.{propertyFirstLetterLowerCase}Id);
                                          }}
                                        }}

                                         get{propertyFirstLetterUpperCase}s(searchModel: {propertyFirstLetterUpperCase}SearchCriteria, selected{propertyFirstLetterUpperCase}Id: number = null): void {{
                                          searchModel.sortAscending = false;
                                          searchModel.sortBy = 'createdDate';
                                          searchModel.companyId = this.companyId;
                                          this.httpService.get{propertyFirstLetterUpperCase}s(searchModel).subscribe({{
                                            next: (data) => {{
                                              if (!data.isSuccess)
                                                return;
                                              this.{propertyFirstLetterLowerCase}s = data.model;
                                              if (!this.isCreateMode) this.selected{propertyFirstLetterUpperCase}s = this.{propertyFirstLetterLowerCase}s.find(item => item.{propertyFirstLetterLowerCase}Id === this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Id);

                                              if (selected{propertyFirstLetterUpperCase}Id) {{
                                                     const selectedItem = this.{propertyFirstLetterLowerCase}s.find(item => item.{propertyFirstLetterLowerCase}Id === selected{propertyFirstLetterUpperCase}Id);
                                                     this.selected{propertyFirstLetterUpperCase}s.push(selectedItem);
                                                }} 
                                            }},
                                            error: (error) => this.dialogService.error('Not Able to get {propertySplittedPascalName}s'),
                                            complete: () => null,
                                          }});
                                        }}
    
                               /////////////////////////// location /////////////////////////////
                               
                                this.{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}Ids = this.selected{propertyFirstLetterLowerCase}s?.map(item => item.{propertyFirstLetterLowerCase}Id);

                            ");
                }
                else
                {
                    throw new Exception("Unknow theme provided");
                }
            }
            else
            {
                if (theme == "velzon")
                {
                    if (propertyType == "text" || propertyType == "email")
                    {
                        var emailValidationMessage = propertyType == "email" ? $@"
                            <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.hasError('emailvalidator')"">
                            Please provide a valid {propertySplittedPascalName}
                          </span>" : "";
                        var appEmailvalidatorStr = propertyType == "email" ? "appEmailvalidator" : "";

                       Console.Write($@"

                              ////////////////////  HTML File  ////////////////////////////// 

                              <div class=""col-sm-6"">
                              <label class=""form-label"">{propertySplittedPascalName}
                                <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                              </label>
                              <div *ngIf=""!isViewMode"">
                                <input type=""text"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required maxlength=""{{{{{propertyFirstLetterLowerCase}__size}}}}""
                                  {appEmailvalidatorStr} class=""form-control"" [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}"" placeholder=""{propertySplittedPascalName}"">
                                <div class=""invalid-feedback invalid-feedback-fixed"">
                                  <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                                    {propertySplittedPascalName} is required
                                  </span>
                                    <span *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.errors?.maxlength"">
                                        Maximum {{{{{propertyFirstLetterLowerCase}__size}}}} characters allowed.
                                    </span>
                                    {emailValidationMessage}
                                </div>
                              </div>
                              <div *ngIf=""isViewMode"">
                                {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} : '-'}}}}
                              </div>
                            </div>

                              ////////////////////  TS File  ////////////////////////////// 

                               {propertyFirstLetterLowerCase}__size: number = {tableFirstNameUpperCase}Model.{propertyFirstLetterLowerCase}__size;
                     
                                    ");
 
                    }
                    if (propertyType == "number")
                    { 
                        Console.Write($@"
                             <div class=""col-sm-6"">
                              <label class=""form-label"">{propertySplittedPascalName}
                                <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                              </label>
                              <div *ngIf=""!isViewMode"">
                                <input type=""number"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required
                                 class=""form-control"" [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}"" placeholder=""{propertySplittedPascalName}"">
                                <div class=""invalid-feedback invalid-feedback-fixed"">
                                  <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                                    {propertySplittedPascalName} is required
                                  </span>
                                </div>
                              </div>
                              <div *ngIf=""isViewMode"">
                                {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} : '-'}}}}
                              </div>
                            </div>  
                                ");

                    }
                    else if (propertyType == "textArea") {

                        Console.Write($@"
                              ////////////////////  HTML File  ////////////////////////////// 

                          <div class=""col-sm-12"">
                            <label class=""form-label"">{propertySplittedPascalName}
                              < span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                            </label>
                            <div *ngIf=""!isViewMode"">
                              <textarea type=""text"" rows=""5"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required
                                 class=""form-control"" [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                                placeholder=""{propertySplittedPascalName}""></textarea>
                              <div class=""invalid-feedback invalid-feedback-fixed"">
                                <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                                  {propertySplittedPascalName} is required
                                </span>
                              </div>
                            </div>
                            <div *ngIf=""isViewMode"">
                              {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} : '-'}}}}
                            </div>
                          </div>

                              ////////////////////  TS File  ////////////////////////////// 

                               {propertyFirstLetterLowerCase}__size: number = {tableFirstNameUpperCase}Model.{propertyFirstLetterLowerCase}__size;
                     
                        ");       
                     }
                    else
                    {
                                throw new Exception($"{propertyType} does not exists");
                    }
            }
            else if (theme == "tabler")
            {

                    if (propertyType == "text" || propertyType == "email")
                    {
                        var emailValidationMessage = propertyType == "email" ? $@"
                            <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.hasError('emailvalidator')"">
                            Please provide a valid {propertySplittedPascalName}
                          </span>" : "";
                        var appEmailvalidatorStr = propertyType == "email" ? "appEmailvalidator" : "";

                        Console.Write($@"
                              ////////////////////  HTML File  ////////////////////////////// 

                              <div class=""col-sm-6"">
                              <label class=""form-label"">{propertySplittedPascalName}
                                <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                              </label>
                              <div *ngIf=""!isViewMode"">
                                <input type=""text"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required maxlength=""{{{{{propertyFirstLetterLowerCase}__size}}}}""
                                  {appEmailvalidatorStr} [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}"" placeholder=""{propertySplittedPascalName}"">
                                <div class=""invalid-feedback invalid-feedback-fixed"">
                                  <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                                    {propertySplittedPascalName} is required
                                  </span>
                                    <span *ngIf=""lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.errors?.maxlength"">
                                        Maximum {{{{{propertyFirstLetterLowerCase}__size}}}} characters allowed.
                                    </span>
                                    {emailValidationMessage}
                                </div>
                              </div>
                              <div *ngIf=""isViewMode"">
                                {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} : '-'}}}}
                              </div>
                            </div>

                              ////////////////////  TS File  ////////////////////////////// 

                              {propertyFirstLetterLowerCase}__size: number = {tableFirstNameUpperCase}Model.{propertyFirstLetterLowerCase}__size;

                                    ");

                    }
                    if (propertyType == "number")
                    {
                        Console.Write($@"
                              <div class=""col-sm-6"">
                              <label class=""form-label"">{propertySplittedPascalName}
                                <span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                              </label>
                              <div *ngIf=""!isViewMode"">
                                <input type=""number"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required
                                 [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}"" placeholder=""{propertySplittedPascalName}"">
                                <div class=""invalid-feedback invalid-feedback-fixed"">
                                  <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                                    {propertySplittedPascalName} is required
                                  </span>
                                </div>
                              </div>
                              <div *ngIf=""isViewMode"">
                                {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} : '-'}}}}
                              </div>
                            </div>
                                    ");

                    }
                    else if (propertyType == "textArea")
                    {

                        Console.Write($@"
                              ////////////////////  HTML File  ////////////////////////////// 

                          <div class=""col-sm-12"">
                            <label class=""form-label"">{propertySplittedPascalName}
                              < span class=""required-sign"" *ngIf=""!isViewMode"">&nbsp;*</span>
                            </label>
                            <div *ngIf=""!isViewMode"">
                              <textarea type=""text"" rows=""5"" #lv{propertyFirstLetterUpperCase}=""ngModel"" [(ngModel)]=""{tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase}"" name=""{propertyFirstLetterLowerCase}"" id=""{propertyFirstLetterLowerCase}"" required
                                 [ngClass]=""{{'is-invalid': lv{tableFirstNameUpperCase}Form.submitted && lv{propertyFirstLetterUpperCase}.invalid }}""
                                placeholder=""{propertySplittedPascalName}""></textarea>
                              <div class=""invalid-feedback invalid-feedback-fixed"">
                                <span *ngIf=""(lv{tableFirstNameUpperCase}Form.submitted) && lv{propertyFirstLetterUpperCase}.errors?.required"">
                                  {propertySplittedPascalName} is required
                                </span>
                              </div>
                            </div>
                            <div *ngIf=""isViewMode"">
                              {{{{{tableFirstNameLowerCase}Model?.{propertyFirstLetterLowerCase} ? {tableFirstNameLowerCase}Model.{propertyFirstLetterLowerCase} : '-'}}}}
                            </div>
                          </div>

                              ////////////////////  TS File  ////////////////////////////// 
                               {propertyFirstLetterLowerCase}__size: number = {tableFirstNameUpperCase}Model.{propertyFirstLetterLowerCase}__size;

                        ");
                    }
                    else
                    {
                        throw new Exception($"{propertyType} does not exists");
                    }
                }
            else
            {
                throw new Exception("Unknow theme provided");
            }
        }
}
         

        static List<string> getInputTypes()
        {
            var result = new List<string>();
            result.Add("text");
            result.Add("email");
            result.Add("number");
            result.Add("checkbox");
            result.Add("date");
            result.Add("time");
            result.Add("predefinedSingleSelect");
            result.Add("singleSelect");
            result.Add("multiSelect");
            result.Add("country");
            result.Add("textArea");
            return result;
        }
    }
}