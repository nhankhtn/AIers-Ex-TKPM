import slugify from "slugify";

export const removeVietnameseTones = (str: string) => {
  return str
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .replace(/đ/g, "d")
    .replace(/Đ/g, "D");
};

export const normalizeString = (str: string) => {
  return slugify(str, { lower: true, locale: "vi" });
};
