"use client";

import { useEffect, useState } from "react";
import { NextIntlClientProvider } from "next-intl";

type Locale = "vi" | "en";
type Messages = Record<Locale, any>;

interface ClientLocaleProviderProps {
  children: React.ReactNode;
  serverLocale: string;
  messages: Messages;
}

export function ClientLocaleProvider({
  children,
  serverLocale,
  messages,
}: ClientLocaleProviderProps) {
  const [locale, setLocale] = useState<Locale>(serverLocale as Locale);

  useEffect(() => {
    const savedLocale = localStorage.getItem("language") as Locale | null;
    if (savedLocale === "en" || savedLocale === "vi") {
      setLocale(savedLocale);
    }
  }, []);

  return (
    <NextIntlClientProvider locale={locale} messages={messages[locale]}>
      {children}
    </NextIntlClientProvider>
  );
}
