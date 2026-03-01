import { Container } from "@/components/ui/Container";
import { SectionTitle } from "@/components/ui/SectionTitle";

export default function Home() {
  return (
    <Container>
      <Container className="flex flex-wrap">
        <div className="flex items-center w-full lg:w-2/3 mx-auto">
          <div className="max-w-2xl mb-8 mx-auto text-center">
            <h1 className="text-4xl font-bold leading-snug tracking-tight text-gray-800 lg:text-4xl lg:leading-tight xl:text-6xl xl:leading-tight dark:text-white">
              AI-Powered Product Tracker
            </h1>
            <p className="py-5 text-xl leading-normal text-gray-500 lg:text-xl xl:text-2xl dark:text-gray-300">
              Describe a product, and let AI search the web, compare your
              options, and lay them out side by side.
            </p>
          </div>
        </div>
      </Container>

      <SectionTitle preTitle="How it works" title="Three steps to smarter comparisons">
        Tell us what you&apos;re looking for. Our AI handles the rest.
      </SectionTitle>

      <Container className="flex flex-wrap gap-8 justify-center mt-4 mb-12">
        <Step number={1} title="Describe" description="Enter a plain-language description of the product you need." />
        <Step number={2} title="Search" description="AI scans the web and gathers matching products with specs and pricing." />
        <Step number={3} title="Compare" description="Review a structured comparison table and pick the best fit." />
      </Container>
    </Container>
  );
}

function Step({ number, title, description }: { number: number; title: string; description: string }) {
  return (
    <div className="flex flex-col items-center w-full sm:w-64 text-center">
      <div className="flex items-center justify-center w-12 h-12 mb-4 text-lg font-bold text-white bg-indigo-600 rounded-full">
        {number}
      </div>
      <h3 className="text-xl font-semibold text-gray-800 dark:text-white">{title}</h3>
      <p className="mt-2 text-gray-500 dark:text-gray-300">{description}</p>
    </div>
  );
}
