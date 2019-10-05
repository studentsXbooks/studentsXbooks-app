import React from "react";
import Link from "./Link";

const Paging = ({
  className,
  next,
  prev,
  currentPage = 1,
  totalPages = 1,
  baseURL
}) => {
  const page = Number(currentPage);
  return (
    <nav className={className} aria-label="Pagination Navigation">
      {page > 1 && prev && (
        <Link to={`${baseURL}${page - 1}`} aria-label="Previos Page">
          Prev
        </Link>
      )}
      <i>
        {page} of {totalPages}
      </i>
      {next && (
        <Link to={`${baseURL}${page + 1}`} aria-label="Next Page">
          Next
        </Link>
      )}
    </nav>
  );
};

export default Paging;
