
import { BaseService } from 'src/app/shared/services/base.service';
import { API_MENU } from 'src/constants/api-menu.const';
import { PagerSetting } from 'src/app/shared/models/layout/pager-setting.model';
import { PlaceTypeEnum } from 'src/app/shared/enums/placeType-enum';
import _map from 'lodash/map';

/**
 * Return list provinces that belong to country has countryId
 * @param countryId 
 * @param baseService 
 * @param api_menu 
 */
export async function getProvinces(countryId: any, baseService: BaseService, api_menu: API_MENU) {

    var searchObj = {
        countryId: countryId,
        placeType: PlaceTypeEnum.Province
    }
    var provinces = await baseService.postAsync(api_menu.Catalogue.CatPlace.query, searchObj, false, false);
    return provinces;
}

/**
 * Return list districts that belong to province-city / country 
 * @param countryId 
 * @param provinceId 
 * @param baseService 
 * @param api_menu 
 */
export async function getDistricts(countryId: any, provinceId: any, baseService: BaseService, api_menu: API_MENU) {
    var baseService: BaseService;
    var api_menu: API_MENU;
    var searchObj = {
        countryId: countryId,
        provinceId: provinceId,
        placeType: PlaceTypeEnum.District
    }
    var districts = await baseService.postAsync(api_menu.Catalogue.CatPlace.query, searchObj, false, false);
    return districts;
}

/**
 * Return list town-ward that belong to district/province/country 
 * @param countryId 
 * @param provinceId 
 * @param districtId 
 * @param pager 
 * @param baseService 
 * @param api_menu 
 */
export async function getTownWards(countryId: any, provinceId: any, districtId, pager: PagerSetting, baseService: BaseService, api_menu: API_MENU) {
    var baseService: BaseService;
    var api_menu: API_MENU;
    var searchObj = {
        countryId: countryId,
        provinceId: provinceId,
        districtId: districtId,
        placeType: PlaceTypeEnum.Ward
    }
    var townWards = await baseService.postAsync(api_menu.Catalogue.CatPlace.paging + "?page=" + pager.currentPage + "&size=" + pager.pageSize, searchObj, false, false);
    return townWards;
}

/**
 *  Prepare data for ng2-select control from dataSource
 *  ng2-select required input data following format {id:'',text:''}
 *  for more information please visit : 
 *  https://valor-software.com/ng2-select/
 * @param dataSource 
 * @param idField 
 * @param textField 
 */
export function prepareNg2SelectData(dataSource: any[], idField: any, textField: any) {
    var returnData = _map(dataSource, function (o) {
        return { id: o[idField], text: o[textField] }
    });
    return returnData;
}

/**
 * Return true if input string contains special characters but false
 * @param str 
 */
export function checkSpecialCharacters(str: String) {
    var reg = /[!@#$%^&*()?":{}|<>]/;
    return reg.test(str.toLowerCase());
}

/**
 * Return UTC datetime
 * @param dateTime 
 */
export function dateTimeToUTC(dateTime: any) {
    try {
        const date = new Date(dateTime.toString() + " UTC");
        return date;
    } catch (error) {
        throw error;
    }
}

