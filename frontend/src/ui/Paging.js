import React from "react";
import { Link } from "@reach/router";

type Props = {
  className: string,
  next: boolean,
  prev: boolean,
  currentPage: number,
  totalPages: number,
  baseURL: string
};

const Paging = ({
  className,
  next,
  prev,
  currentPage = 1,
  totalPages = 1,
  baseURL
}: Props) => {
  const page = Number(currentPage);
  return (
    <nav className={className} aria-label="Pagination Navigation">
      {page > 1 && prev && (
        // $FlowFixMe
        <Link to={`${baseURL}${page - 1}`} aria-label="Previous Page">
          Prev
        </Link>
      )}
      <i>
        {page} of {totalPages}
      </i>
      {next && (
        // $FlowFixMe
        <Link to={`${baseURL}${page + 1}`} aria-label="Next Page">
          Next
        </Link>
      )}
    </nav>
  );
};

export default Paging;
