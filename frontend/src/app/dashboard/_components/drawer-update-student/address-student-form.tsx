"use client";

import { AddressApi } from "@/api/address";
import RowStack from "@/components/row-stack";
import useFunction from "@/hooks/use-function";
import { Country } from "@/types/address";
import { InfoOutlined } from "@mui/icons-material";
import {
  Typography,
  FormControlLabel,
  Checkbox,
  Box,
  Tooltip,
} from "@mui/material";
import { useEffect, useMemo } from "react";
import PermanentAddress from "./permanent-address";
import TemporaryAddress from "./temporary-address";
import MailingAddress from "./mailing-address";
import { useMainContext } from "@/context/main/main-context";
import { useTranslations } from "next-intl";

const AddressStudentForm = ({
  formik,
  open,
  countries,
}: {
  formik: any;
  open: boolean;
  countries: Country[];
}) => {
  const t = useTranslations("dashboard.list");
  const { provinces } = useMainContext();
  const getDistrictOfProvincesPAApi = useFunction(
    AddressApi.getDistrictOfProvinces
  );
  const getDistrictOfProvincesTAApi = useFunction(
    AddressApi.getDistrictOfProvinces
  );
  const getDistrictOfProvincesMAApi = useFunction(
    AddressApi.getDistrictOfProvinces
  );
  const getWardOfDistrictPAApi = useFunction(AddressApi.getWardOfDistrict);
  const getWardOfDistrictTAApi = useFunction(AddressApi.getWardOfDistrict);
  const getWardOfDistrictMAApi = useFunction(AddressApi.getWardOfDistrict);

  const districtsPA = useMemo(
    () => getDistrictOfProvincesPAApi.data?.districts || [],
    [getDistrictOfProvincesPAApi.data]
  );
  const districtsTA = useMemo(
    () => getDistrictOfProvincesTAApi.data?.districts || [],
    [getDistrictOfProvincesTAApi.data]
  );

  const districtsMA = useMemo(
    () => getDistrictOfProvincesMAApi.data?.districts || [],
    [getDistrictOfProvincesMAApi.data]
  );

  const wardsPA = useMemo(
    () => getWardOfDistrictPAApi.data?.wards || [],
    [getWardOfDistrictPAApi.data]
  );
  const wardsTA = useMemo(
    () => getWardOfDistrictTAApi.data?.wards || [],
    [getWardOfDistrictTAApi.data]
  );
  const wardsMA = useMemo(
    () => getWardOfDistrictMAApi.data?.wards || [],
    [getWardOfDistrictMAApi.data]
  );
  useEffect(() => {
    if (formik.values.permanentProvince) {
      const province = provinces.find(
        (province) => province.name === formik.values.permanentProvince
      );
      if (!province) return;

      getDistrictOfProvincesPAApi.call({ province_code: province.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.permanentProvince, provinces]);
  useEffect(() => {
    if (formik.values.temporaryProvince) {
      const province = provinces.find(
        (province) => province.name === formik.values.temporaryProvince
      );
      if (!province) return;

      getDistrictOfProvincesTAApi.call({ province_code: province.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.temporaryProvince, provinces]);

  useEffect(() => {
    if (formik.values.mailingProvince) {
      const province = provinces.find(
        (province) => province.name === formik.values.mailingProvince
      );
      if (!province) return;

      getDistrictOfProvincesMAApi.call({ province_code: province.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.mailingProvince, provinces]);

  useEffect(() => {
    if (formik.values.permanentDistrict) {
      const district = districtsPA.find(
        (district) => district.name === formik.values.permanentDistrict
      );
      if (!district) return;

      getWardOfDistrictPAApi.call({ district_code: district.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.permanentDistrict, districtsPA]);
  useEffect(() => {
    if (formik.values.temporaryDistrict) {
      const district = districtsTA.find(
        (district) => district.name === formik.values.temporaryDistrict
      );
      if (!district) return;

      getWardOfDistrictTAApi.call({ district_code: district.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.temporaryDistrict, districtsTA]);
  useEffect(() => {
    if (formik.values.mailingDistrict) {
      const district = districtsMA.find(
        (district) => district.name === formik.values.mailingDistrict
      );
      if (!district) return;

      getWardOfDistrictMAApi.call({ district_code: district.code });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [formik.values.mailingDistrict, districtsMA]);

  return (
    <>
      <Typography variant='h6'>{t("permanentAddress")}</Typography>
      <PermanentAddress
        formik={formik}
        countries={countries}
        provinces={provinces}
        districtsPA={districtsPA}
        wardsPA={wardsPA}
      />
      {/* Temporary Address */}
      <Box>
        <RowStack>
          <FormControlLabel
            control={
              <Checkbox
                checked={formik.values.useTemporaryAddress}
                onChange={() => {
                  formik.setFieldValue(
                    "useTemporaryAddress",
                    !formik.values.useTemporaryAddress
                  );
                  formik.validateForm();
                }}
                name='useTemporaryAddress'
              />
            }
            label={t("temporaryAddress")}
          />
          <Tooltip title={t("temporaryAddressTooltip")} placement='top'>
            <InfoOutlined
              sx={{
                color: "action.active",
              }}
            />
          </Tooltip>
        </RowStack>

        {formik.values.useTemporaryAddress && (
          <TemporaryAddress
            formik={formik}
            countries={countries}
            provinces={provinces}
            districtsTA={districtsTA}
            wardsTA={wardsTA}
          />
        )}
      </Box>

      {/* Mailing Address */}
      <Box>
        <RowStack>
          <FormControlLabel
            control={
              <Checkbox
                checked={formik.values.useMailingAddress}
                onChange={() => {
                  formik.setFieldValue(
                    "useMailingAddress",
                    !formik.values.useMailingAddress
                  );
                  formik.validateForm();
                }}
                name='useMailingAddress'
              />
            }
            label={t("mailingAddress")}
          />
          <Tooltip title={t("mailingAddressTooltip")} placement='top'>
            <InfoOutlined
              sx={{
                color: "action.active",
              }}
            />
          </Tooltip>
        </RowStack>

        {formik.values.useMailingAddress && (
          <MailingAddress
            formik={formik}
            countries={countries}
            provinces={provinces}
            districtsMA={districtsMA}
            wardsMA={wardsMA}
          />
        )}
      </Box>
    </>
  );
};

export default AddressStudentForm;
