import Image from "next/image";
import React from "react";
import { Container } from "@/components/ui/Container";

export function Footer() {
  return (
    <div className="relative">
      <Container>
        <div className="flex flex-col items-center justify-center gap-4 pt-10 pb-10 mx-auto mt-5 border-t border-gray-100 dark:border-trueGray-700 sm:flex-row sm:gap-8">
          <a
            href="https://www.linkedin.com/in/yusef99/"
            target="_blank"
            rel="noopener noreferrer"
            className="flex items-center space-x-2 text-gray-500 dark:text-gray-400 hover:text-indigo-500 dark:hover:text-indigo-400"
          >
            <Image
              src="/img/linkedin.png"
              alt="LinkedIn"
              width={24}
              height={24}
            />
            <span>Contact the Developer</span>
          </a>
          <a
            href="https://github.com/ynr2321/.net-aspire-next.js-ai-product-tracker"
            target="_blank"
            rel="noopener noreferrer"
            className="flex items-center space-x-2 text-gray-500 dark:text-gray-400 hover:text-indigo-500 dark:hover:text-indigo-400"
          >
            <Image
              src="/img/githubLogo.png"
              alt="GitHub"
              width={24}
              height={24}
            />
            <span>View on GitHub</span>
          </a>
        </div>
      </Container>
    </div>
  );
}
