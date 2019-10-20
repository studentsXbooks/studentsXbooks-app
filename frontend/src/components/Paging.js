import React from "react";
import { Link } from "@reach/router";
import { Button, Typography, Grid } from "@material-ui/core";

type Props = {
  className?: string,
  currentPage: string,
  totalPages: string,
  basePath: string
};

const Paging = ({
  className,
  currentPage = "1",
  totalPages = "1",
  basePath
}: Props) => {
  const page = Number(currentPage);
  return (
    <nav className={className} aria-label="Pagination Navigation">
      <Grid container justify="center" spacing={3}>
        <Grid item>
          <Button variant="contained" color="secondary">
            {/* // $FlowFixMe */}
            <Link
              to={`${basePath}/${page - 1}${window.location.search}`}
              aria-label="Previous Page"
              aria-disabled={page <= 1}
            >
              Prev
            </Link>
          </Button>
        </Grid>
        <Grid item>
          <Typography>
            {page} of {totalPages}
          </Typography>
        </Grid>
        <Grid item>
          <Button variant="contained" color="primary">
            {/* // $FlowFixMe */}
            <Link
              to={`${basePath}/${page + 1}${window.location.search}`}
              aria-label="Next Page"
              aria-disabled={page >= Number(totalPages)}
            >
              Next
            </Link>
          </Button>
        </Grid>
      </Grid>
    </nav>
  );
};

Paging.defaultProps = {
  className: ""
};

export default Paging;
