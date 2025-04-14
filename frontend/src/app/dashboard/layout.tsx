"use client";

import MainProvider from "@/context";

export default function Layout({ children }: { children: React.ReactNode }) {

  return (
      <MainProvider>{children}</MainProvider>
  );
}
