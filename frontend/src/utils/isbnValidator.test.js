import { isISBN10, isISBN13 } from "./isbnValidator";

describe("isbn10Validator", () => {
  const goodInput = [
    "0802122280",
    "1617290653",
    "0981531687",
    "0-306-40615-2",
    "0-04-313341-X"
  ];

  const badInput = ["0", "01111111111111111111", "1234567890"];
  goodInput.forEach(input => {
    test(`Should return true with input=${input}`, () => {
      expect(isISBN10(input)).toBe(true);
    });
  });

  badInput.forEach(input => {
    test(`Should return false with input=${input}`, () => {
      expect(isISBN10(input)).toBe(false);
    });
  });
});

describe("isb13Validator", () => {
  const goodInput = [
    "978-0-306-406157",
    "9780132350884",
    "978-0393912692",
    "9781974305032",
    "978-0-1359-5705-9"
  ];

  const badInput = ["0", "01111111111111111111", "1234567890123"];

  goodInput.forEach(input => {
    test(`Should return true with input=${input}`, () => {
      expect(isISBN13(input)).toBe(true);
    });
  });

  badInput.forEach(input => {
    test(`Should return false with input=${input}`, () => {
      expect(isISBN13(input)).toBe(false);
    });
  });
});
